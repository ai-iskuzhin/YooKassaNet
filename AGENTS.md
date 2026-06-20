# YooKassaNet Working Agreement

This file gives AI coding agents and maintainers the project-specific rules that matter most.

## Purpose

`YooKassaNet` is a .NET SDK for the YooKassa (ЮKassa) API v3. It provides typed clients,
request/response models, and notification parsing for payments, refunds, payouts, safe deals, and webhooks.
It should not contain application-specific business rules, ASP.NET Core endpoints, ORMs, or persistence.

## Package boundary: one package, organized by area

Payments, payouts and deals are a single API (one host `api.yookassa.ru/v3`, one version) sharing the same
primitives. Ship them as **one** package, `YooKassaNet`, organized into folders:

```text
src/YooKassaNet/Common    shared HTTP/auth/idempotency/JSON, Money, enums, errors, receipts, confirmation
src/YooKassaNet/Payments  payments + refunds
src/YooKassaNet/Payouts   payouts + SBP banks + personal data
src/YooKassaNet/Deals     safe deals
src/YooKassaNet/Webhooks  webhook management + incoming notifications
```

Do **not** split into `YooKassaPaymentsNet` / `YooKassaPayoutsNet` / `YooKassaDealsNet` or add a shared
`Abstractions` package — the shared core lives in `Common`. Keep each area's client a separate public type
(`YooKassaPaymentsClient`, etc.) so payouts can use distinct gateway credentials.

## Targets

`netstandard2.0;net8.0;net10.0`. PolySharp provides modern language features on `netstandard2.0`;
`System.Text.Json` / `System.Net.Http.Json` are referenced only there (in-box on net8.0+).

## Enum rules (important)

Every wire enum is `[YooKassaEnum]` with one `[YooKassaWireName("...")]` per member. Serialization goes
through `YooKassaEnumConverterFactory`. An **unmapped** value (read or write) throws
`YooKassaProtocolException` via `YooKassaWireParsing.UnknownEnumValue`, which links to
`https://github.com/ai-iskuzhin/YooKassaNet/issues/new`. When you learn of a new API value, add the member.

Deliberate exceptions kept as `string` (not enums) to avoid throwing on benign/open sets: error `code`,
card brand (`card_type`), and `/me` `payment_methods`.

## Conventions

- Preserve YooKassa wire names. JSON uses snake_case (`JsonNamingPolicy.SnakeCaseLower`); add explicit
  `[JsonPropertyName]` for fields with digits (`first6`, `last4`) or other non-obvious mappings.
- `Money` serializes `value` as a 2-decimal string; amounts are `decimal` in code.
- Every write sends an `Idempotence-Key` (auto-generated `Guid` unless the caller passes one).
- Non-2xx → `YooKassaApiException` with the parsed `YooKassaError`.
- Russian XML doc comments for public domain types, enums, and methods. Every method links to its
  YooKassa API anchor: `https://yookassa.ru/developers/api#<navKey>` (e.g. `#create_payment`).
- `GenerateDocumentationFile` is on and `CS1591` is an error: **document every public member**.

## Testing

- Unit tests (`tests/YooKassaNet.Tests`): offline, drive clients through a stubbed `HttpMessageHandler`;
  assert request method/path/headers/body and response/enum/error parsing.
- Integration tests (`tests/YooKassaNet.Tests.Integration`): live against the YooKassa test environment;
  read credentials from env vars (`YOOKASSA_*`) and **skip silently** when absent. Never commit real
  credentials; `.env` is gitignored.

Always run:

```bash
dotnet test YooKassaNet.slnx
```

For package-facing changes also run:

```bash
dotnet pack YooKassaNet.slnx --configuration Release --output artifacts/packages
```

## Release discipline

Semantic Versioning. Preview versions like `0.1.0-preview.1`; tags `v<version>`. Do not call the package
`1.0.0` until a real integration validates the public API shape. Update `CHANGELOG.md` for notable changes.

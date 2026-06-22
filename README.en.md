<table>
  <tr>
    <td width="170" align="center" valign="middle">
      <img src="assets/logo.png" width="140" alt="YooKassaNet logo" />
    </td>
    <td valign="middle">
      <h1>YooKassaNet</h1>
      <p>.NET SDK for the <a href="https://yookassa.ru/developers/api">YooKassa (ЮKassa) API v3</a>: <strong>payments</strong>, <strong>refunds</strong>, <strong>payouts</strong>, and <strong>safe deals</strong>, plus webhooks and shop settings — shipped as a <strong>single package</strong>.</p>
      <p>
        <a href="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/ci.yml"><img src="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/ci.yml/badge.svg?branch=main" alt="CI" /></a>
        <a href="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/release.yml"><img src="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/release.yml/badge.svg" alt="Release" /></a>
        <a href="https://github.com/ai-iskuzhin/YooKassaNet/blob/main/LICENSE"><img src="https://img.shields.io/github/license/ai-iskuzhin/YooKassaNet?style=flat-square" alt="License" /></a>
        <a href="https://www.nuget.org/packages/YooKassaNet"><img src="https://img.shields.io/badge/targets-netstandard2.0%20%7C%20net8.0%20%7C%20net10.0-512BD4?logo=dotnet&amp;style=flat-square" alt="Targets" /></a>
      </p>
      <p>
        <a href="https://www.nuget.org/packages/YooKassaNet"><img src="https://img.shields.io/nuget/v/YooKassaNet?logo=nuget&amp;style=flat-square" alt="NuGet version" /></a>
        <a href="https://www.nuget.org/packages/YooKassaNet"><img src="https://img.shields.io/nuget/dt/YooKassaNet?style=flat-square" alt="NuGet downloads" /></a>
        <a href="README.md"><img src="https://img.shields.io/badge/README-Русский-0058D6?logo=googletranslate&amp;logoColor=white&amp;style=flat-square" alt="Читать на русском" /></a>
      </p>
    </td>
  </tr>
</table>

- Targets `netstandard2.0`, `net8.0`, `net10.0`.
- Only `HttpClient` + `System.Text.Json`.

> Payments, payouts and deals are one API (same host, one `v3` version) sharing the same primitives — `Money`, idempotency, error model, cursor pagination. They ship together as `YooKassaNet`, organized internally by area, with a separate typed client per area so payouts can use their own gateway credentials.

## Install

```bash
dotnet add package YooKassaNet
```

## Authentication

| Area | Credentials |
| --- | --- |
| Payments, deals, webhooks | shop `shopId` + secret key |
| Payouts | payout gateway (agent) id + payout secret key (usually **different** credentials) |

OAuth tokens are also supported via `YooKassaClientOptions.OAuthToken`. See [Authentication](https://yookassa.ru/developers/using-api/interaction-format#auth).

## Quick start — payments

```csharp
using YooKassaNet;
using YooKassaNet.Payments;

using var http = new HttpClient();
var payments = new YooKassaPaymentsClient(http, new YooKassaClientOptions
{
    ShopId = "1281498",
    SecretKey = "test_...",
});

// Create a one-stage payment and redirect the customer to confirmation.
var payment = await payments.CreatePaymentAsync(new CreatePaymentRequest
{
    Amount = Money.Rubles(100m),
    Capture = true,
    Confirmation = Confirmation.Redirect("https://example.com/return"),
    Description = "Order #37",
});

Console.WriteLine(payment.Confirmation?.ConfirmationUrl);

// Later: read it back and refund if needed.
var fresh = await payments.GetPaymentAsync(payment.Id);

var refund = await payments.CreateRefundAsync(new CreateRefundRequest
{
    PaymentId = payment.Id,
    Amount = Money.Rubles(100m),
});
```

Two-stage payments: create with `Capture = false`, then `CapturePaymentAsync(id, ...)` or `CancelPaymentAsync(id)`.

## Payouts

```csharp
using YooKassaNet.Payouts;

var payouts = new YooKassaPayoutsClient(http, new YooKassaClientOptions
{
    ShopId = "513961",      // payout gateway (agent) id
    SecretKey = "test_...", // payout secret key
});

var payout = await payouts.CreatePayoutAsync(new CreatePayoutRequest
{
    Amount = Money.Rubles(320m),
    PayoutDestinationData = new PayoutDestinationData
    {
        Type = PayoutDestinationType.BankCard,
        Card = new PayoutCardData { Number = "5555555555554444" },
    },
    Description = "Payout for order #37",
});

var banks = await payouts.GetSbpBanksAsync();
```

## Safe deals

```csharp
using YooKassaNet.Deals;

var deals = new YooKassaDealsClient(http, options);
var deal = await deals.CreateDealAsync(new CreateDealRequest
{
    FeeMoment = FeeMoment.PaymentSucceeded,
    Description = "SAFE_DEAL order #37",
});
```

A deal binds a payment to a payout: pass `new PaymentDeal { Id = deal.Id, ... }` on the payment and `new PayoutDeal { Id = deal.Id }` on the payout.

## One facade for everything

```csharp
var yoo = new YooKassaClient(
    http,
    new YooKassaClientOptions { ShopId = "1281498", SecretKey = "test_..." },  // payments / deals / webhooks
    new YooKassaClientOptions { ShopId = "513961",  SecretKey = "test_..." }); // payouts (optional)

await yoo.Payments.CreatePaymentAsync(/* ... */);
await yoo.Payouts.CreatePayoutAsync(/* ... */);
var me = await yoo.GetMeAsync();
```

## Incoming webhooks

```csharp
using YooKassaNet.Webhooks;

var notification = YooKassaNotification.Parse(requestBody);
switch (notification.Event)
{
    case WebhookEvent.PaymentSucceeded:
        var paid = notification.AsPayment();
        // Always re-fetch from the API before acting on a notification.
        break;
}
```

## Idempotency

Every write generates an `Idempotence-Key` automatically. To make a retry safe, pass your own key:

```csharp
await payments.CreatePaymentAsync(request, idempotenceKey: "order-37-create");
```

## Error handling

| Exception | When |
| --- | --- |
| `YooKassaApiException` | API returned a non-2xx response. `.Error` holds the parsed [error object](https://yookassa.ru/developers/using-api/interaction-format#error-object). |
| `YooKassaTransportException` | Network failure before a response was received. |
| `YooKassaProtocolException` | Response could not be parsed — including an **enum value this SDK doesn't know yet** (please [report it](https://github.com/ai-iskuzhin/YooKassaNet/issues/new)). |
| `YooKassaValidationException` | Local validation failed before sending. |

## Supported API

YooKassaNet provides typed support for:

**Payments and refunds** (`YooKassaPaymentsClient`)
- [Create payment](https://yookassa.ru/developers/api#create_payment) — `CreatePaymentAsync`
- [Get payment](https://yookassa.ru/developers/api#get_payment) — `GetPaymentAsync`
- [List payments](https://yookassa.ru/developers/api#get_payments_list) — `GetPaymentsAsync`
- [Capture payment](https://yookassa.ru/developers/api#capture_payment) — `CapturePaymentAsync`
- [Cancel payment](https://yookassa.ru/developers/api#cancel_payment) — `CancelPaymentAsync`
- [Create refund](https://yookassa.ru/developers/api#create_refund) — `CreateRefundAsync`
- [Get refund](https://yookassa.ru/developers/api#get_refund) — `GetRefundAsync`
- [List refunds](https://yookassa.ru/developers/api#get_refunds_list) — `GetRefundsAsync`

**Payouts** (`YooKassaPayoutsClient`)
- [Create payout](https://yookassa.ru/developers/api#create_payout) — `CreatePayoutAsync`
- [Get payout](https://yookassa.ru/developers/api#get_payout) — `GetPayoutAsync`
- [List SBP banks](https://yookassa.ru/developers/api#get_sbp_banks_list) — `GetSbpBanksAsync`
- [Create personal data](https://yookassa.ru/developers/api#create_personal_data) — `CreatePersonalDataAsync`
- [Get personal data](https://yookassa.ru/developers/api#get_personal_data) — `GetPersonalDataAsync`

**Safe deals** (`YooKassaDealsClient`)
- [Create deal](https://yookassa.ru/developers/api#create_deal) — `CreateDealAsync`
- [Get deal](https://yookassa.ru/developers/api#get_deal) — `GetDealAsync`
- [List deals](https://yookassa.ru/developers/api#get_deals_list) — `GetDealsAsync`

**Webhooks** (`YooKassaWebhooksClient`)
- [Create webhook](https://yookassa.ru/developers/api#create_webhook) — `CreateWebhookAsync`
- [List webhooks](https://yookassa.ru/developers/api#get_webhook_list) — `GetWebhooksAsync`
- [Delete webhook](https://yookassa.ru/developers/api#delete_webhook) — `DeleteWebhookAsync`
- [Parse incoming notifications](https://yookassa.ru/developers/using-api/webhooks) — `YooKassaNotification.Parse`

**Shop**
- [Get shop settings](https://yookassa.ru/developers/api#get_me) — `GetMeAsync` (on `YooKassaClient`)

Not yet exposed as dedicated methods: [payment methods](https://yookassa.ru/developers/api#payment_method_object), [invoices](https://yookassa.ru/developers/api#invoice_object), [receipts](https://yookassa.ru/developers/api#receipt_object), and [payout search](https://yookassa.ru/developers/api#get_payouts_search). 54-FZ receipts can be sent inline on payments, refunds, and payouts via the `Receipt` property.

## Repository layout

```text
src/YooKassaNet/
  Common/    shared core: HTTP, auth, idempotency, JSON, enums, Money, errors, receipts, confirmation
  Payments/  payments + refunds
  Payouts/   payouts + SBP banks + personal data
  Deals/     safe deals
  Webhooks/  webhook management + incoming notification parsing
tests/
  YooKassaNet.Tests/             unit tests (offline, stubbed HttpClient)
  YooKassaNet.Tests.Integration/ live tests against the YooKassa test environment (opt-in via env vars)
docs/
  yookassa-api-spec.json
```

## Build and test

```bash
dotnet build YooKassaNet.slnx -c Release
dotnet test  YooKassaNet.slnx -c Release
dotnet pack  YooKassaNet.slnx -c Release -o artifacts/packages
```

## Integration tests

Integration tests hit the YooKassa **test** environment and are **skipped automatically** unless credentials are present. Provide them via environment variables (or a gitignored `.env` file — see [.env.example](.env.example), auto-loaded by the integration test project):

| Variable | Meaning |
| --- | --- |
| `YOOKASSA_SHOP_ID` / `YOOKASSA_SECRET_KEY` | shop credentials (payments, deals, webhooks) |
| `YOOKASSA_PAYOUT_AGENT_ID` / `YOOKASSA_PAYOUT_SECRET_KEY` | payout gateway credentials |
| `YOOKASSA_BASE_URL` | optional base address override |

```bash
cp .env.example .env   # then fill in test credentials
dotnet test tests/YooKassaNet.Tests.Integration -c Release
```

## Versioning

Semantic Versioning. Pre-1.0 the public API may change; the first stable `1.0.0` ships after a real integration validates the surface. Release tags are `v<version>` (e.g. `v0.1.0-preview.1`).

## License

[MIT](LICENSE).

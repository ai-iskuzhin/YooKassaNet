# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and the project follows
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0-preview.1] - 2026-06-20

### Added

- Initial SDK skeleton as a single package `YooKassaNet` targeting `netstandard2.0`, `net8.0`, `net10.0`.
- Shared core: `YooKassaApiClient` (auth, idempotency, JSON, error handling), `Money`/`Currency`,
  `YooKassaClientOptions`, exception hierarchy, `YooKassaList<T>`, `Confirmation`, `CancellationDetails`,
  `Receipt`, `CardInfo`, and the throwing enum converter (`YooKassaEnumConverterFactory`).
- Payments: `YooKassaPaymentsClient` (create / get / list / capture / cancel) and refunds (create / get / list).
- Payouts: `YooKassaPayoutsClient` (create / get payout, SBP banks, personal data).
- Deals: `YooKassaDealsClient` (create / get / list).
- Webhooks: `YooKassaWebhooksClient` (create / list / delete) and `YooKassaNotification` parsing.
- `YooKassaClient` facade and `/me` shop settings.

[Unreleased]: https://github.com/ai-iskuzhin/YooKassaNet/compare/v0.1.0-preview.1...HEAD
[0.1.0-preview.1]: https://github.com/ai-iskuzhin/YooKassaNet/releases/tag/v0.1.0-preview.1

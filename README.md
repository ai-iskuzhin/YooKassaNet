<p align="center">
  <img src="assets/logo.png" alt="YooKassaNet" width="140" />
</p>

<h1 align="center">YooKassaNet</h1>

<p align="center">
  <a href="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/ci.yml"><img src="https://github.com/ai-iskuzhin/YooKassaNet/actions/workflows/ci.yml/badge.svg" alt="CI" /></a>
  <a href="https://www.nuget.org/packages/YooKassaNet"><img src="https://img.shields.io/nuget/v/YooKassaNet.svg" alt="NuGet" /></a>
</p>

🇬🇧 English version: [README.en.md](README.en.md).

.NET SDK для [API ЮKassa v3](https://yookassa.ru/developers/api): **платежи**, **возвраты**, **выплаты** и **безопасные сделки**, а также webhook и настройки магазина — поставляется **одним пакетом**.

- Таргеты: `netstandard2.0`, `net8.0`, `net10.0`.
- Только `HttpClient` + `System.Text.Json` (без сторонних зависимостей).
- Типизированные перечисления для всех значений протокола. Неизвестное значение из API приводит к `YooKassaProtocolException` со ссылкой на [issues/new](https://github.com/ai-iskuzhin/YooKassaNet/issues/new), чтобы его можно было быстро добавить.

> Платежи, выплаты и сделки — это один API (общий хост, одна версия `v3`) с общими примитивами: `Money`, идемпотентность, модель ошибок, курсорная постраничность. Они поставляются вместе как `YooKassaNet`, разделенные внутри по областям, с отдельным типизированным клиентом на каждую область — чтобы выплаты могли использовать свои учетные данные шлюза.

## Установка

```bash
dotnet add package YooKassaNet
```

## Аутентификация

| Область | Учетные данные |
| --- | --- |
| Платежи, сделки, webhook | `shopId` магазина + секретный ключ |
| Выплаты | идентификатор шлюза выплат (агента) + ключ выплат (обычно **отдельные** учетные данные) |

Также поддерживаются OAuth-токены через `YooKassaClientOptions.OAuthToken`. См. [Аутентификацию](https://yookassa.ru/developers/using-api/interaction-format#auth).

## Быстрый старт — платежи

```csharp
using YooKassaNet;
using YooKassaNet.Payments;

using var http = new HttpClient();
var payments = new YooKassaPaymentsClient(http, new YooKassaClientOptions
{
    ShopId = "1281498",
    SecretKey = "test_...",
});

// Создаем одностадийный платеж и отправляем покупателя на страницу подтверждения.
var payment = await payments.CreatePaymentAsync(new CreatePaymentRequest
{
    Amount = Money.Rubles(100m),
    Capture = true,
    Confirmation = Confirmation.Redirect("https://example.com/return"),
    Description = "Заказ №37",
});

Console.WriteLine(payment.Confirmation?.ConfirmationUrl);

// Позже: читаем платеж обратно и при необходимости делаем возврат.
var fresh = await payments.GetPaymentAsync(payment.Id);

var refund = await payments.CreateRefundAsync(new CreateRefundRequest
{
    PaymentId = payment.Id,
    Amount = Money.Rubles(100m),
});
```

Двухстадийный платеж: создайте с `Capture = false`, затем `CapturePaymentAsync(id, ...)` или `CancelPaymentAsync(id)`.

## Выплаты

```csharp
using YooKassaNet.Payouts;

var payouts = new YooKassaPayoutsClient(http, new YooKassaClientOptions
{
    ShopId = "513961",      // идентификатор шлюза выплат (агента)
    SecretKey = "test_...", // ключ выплат
});

var payout = await payouts.CreatePayoutAsync(new CreatePayoutRequest
{
    Amount = Money.Rubles(320m),
    PayoutDestinationData = new PayoutDestinationData
    {
        Type = PayoutDestinationType.BankCard,
        Card = new PayoutCardData { Number = "5555555555554444" },
    },
    Description = "Выплата по заказу №37",
});

var banks = await payouts.GetSbpBanksAsync();
```

## Безопасные сделки

```csharp
using YooKassaNet.Deals;

var deals = new YooKassaDealsClient(http, options);
var deal = await deals.CreateDealAsync(new CreateDealRequest
{
    FeeMoment = FeeMoment.PaymentSucceeded,
    Description = "SAFE_DEAL заказ №37",
});
```

Сделка связывает платеж и выплату: передайте `new PaymentDeal { Id = deal.Id, ... }` в платеж и `new PayoutDeal { Id = deal.Id }` в выплату.

## Единый фасад

```csharp
var yoo = new YooKassaClient(
    http,
    new YooKassaClientOptions { ShopId = "1281498", SecretKey = "test_..." },  // платежи / сделки / webhook
    new YooKassaClientOptions { ShopId = "513961",  SecretKey = "test_..." }); // выплаты (необязательно)

await yoo.Payments.CreatePaymentAsync(/* ... */);
await yoo.Payouts.CreatePayoutAsync(/* ... */);
var me = await yoo.GetMeAsync();
```

## Входящие webhook

```csharp
using YooKassaNet.Webhooks;

var notification = YooKassaNotification.Parse(requestBody);
switch (notification.Event)
{
    case WebhookEvent.PaymentSucceeded:
        var paid = notification.AsPayment();
        // Всегда перепроверяйте объект запросом к API, прежде чем выполнять действия.
        break;
}
```

## Идемпотентность

Для каждой операции записи `Idempotence-Key` генерируется автоматически. Чтобы повтор был безопасным, передайте свой ключ:

```csharp
await payments.CreatePaymentAsync(request, idempotenceKey: "order-37-create");
```

## Обработка ошибок

| Исключение | Когда |
| --- | --- |
| `YooKassaApiException` | API вернул ответ вне диапазона 2xx. `.Error` содержит разобранный [объект ошибки](https://yookassa.ru/developers/using-api/interaction-format#error-object). |
| `YooKassaTransportException` | Сетевой сбой до получения ответа. |
| `YooKassaProtocolException` | Ответ не удалось разобрать — в том числе **значение перечисления, неизвестное SDK** (пожалуйста, [сообщите](https://github.com/ai-iskuzhin/YooKassaNet/issues/new)). |
| `YooKassaValidationException` | Локальная валидация не пройдена до отправки. |

## Структура репозитория

```text
src/YooKassaNet/
  Common/    общее ядро: HTTP, аутентификация, идемпотентность, JSON, перечисления, Money, ошибки, чеки, подтверждение
  Payments/  платежи + возвраты
  Payouts/   выплаты + участники СБП + персональные данные
  Deals/     безопасные сделки
  Webhooks/  управление webhook + разбор входящих уведомлений
tests/
  YooKassaNet.Tests/             модульные тесты (офлайн, через заглушку HttpClient)
  YooKassaNet.Tests.Integration/ боевые тесты на тестовой среде ЮKassa (включаются переменными окружения)
docs/
  yookassa-api-spec.json
```

## Сборка и тесты

```bash
dotnet build YooKassaNet.slnx -c Release
dotnet test  YooKassaNet.slnx -c Release
dotnet pack  YooKassaNet.slnx -c Release -o artifacts/packages
```

## Интеграционные тесты

Интеграционные тесты обращаются к **тестовой** среде ЮKassa и **пропускаются автоматически**, если учетные данные не заданы. Передайте их через переменные окружения (или через gitignore-файл `.env` — см. [.env.example](.env.example); он автоматически загружается проектом интеграционных тестов):

| Переменная | Назначение |
| --- | --- |
| `YOOKASSA_SHOP_ID` / `YOOKASSA_SECRET_KEY` | учетные данные магазина (платежи, сделки, webhook) |
| `YOOKASSA_PAYOUT_AGENT_ID` / `YOOKASSA_PAYOUT_SECRET_KEY` | учетные данные шлюза выплат |
| `YOOKASSA_BASE_URL` | необязательное переопределение базового адреса |

```bash
cp .env.example .env   # затем заполните тестовыми учетными данными
dotnet test tests/YooKassaNet.Tests.Integration -c Release
```

## Версионирование

Semantic Versioning. До версии 1.0 публичный API может меняться; первая стабильная `1.0.0` выйдет после проверки на реальной интеграции. Теги релизов — `v<версия>` (например, `v0.1.0-preview.1`).

## Лицензия

[MIT](LICENSE).

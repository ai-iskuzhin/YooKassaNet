# YooKassaNet

.NET SDK для [API ЮKassa v3](https://yookassa.ru/developers/api): **платежи**, **возвраты**, **выплаты** и **безопасные сделки**, а также webhook и настройки магазина — в одном пакете с общим ядром (HTTP, аутентификация, идемпотентность) и типизированными моделями.

- Таргеты: `netstandard2.0`, `net8.0`, `net10.0`.
- Только `HttpClient` + `System.Text.Json`.

🇬🇧 English version: [README.en.md](https://github.com/ai-iskuzhin/YooKassaNet/blob/main/README.en.md).

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

// Позже: читаем платеж обратно.
var fresh = await payments.GetPaymentAsync(payment.Id);

// Возврат.
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

## Поддерживаемое API

**Платежи и возвраты** (`YooKassaPaymentsClient`): [создание](https://yookassa.ru/developers/api#create_payment), [информация](https://yookassa.ru/developers/api#get_payment), [список](https://yookassa.ru/developers/api#get_payments_list), [подтверждение](https://yookassa.ru/developers/api#capture_payment), [отмена](https://yookassa.ru/developers/api#cancel_payment) платежа; [создание](https://yookassa.ru/developers/api#create_refund), [информация](https://yookassa.ru/developers/api#get_refund), [список](https://yookassa.ru/developers/api#get_refunds_list) возвратов.

**Выплаты** (`YooKassaPayoutsClient`): [создание](https://yookassa.ru/developers/api#create_payout) и [информация](https://yookassa.ru/developers/api#get_payout) о выплате, [участники СБП](https://yookassa.ru/developers/api#get_sbp_banks_list), [создание](https://yookassa.ru/developers/api#create_personal_data) и [информация](https://yookassa.ru/developers/api#get_personal_data) о персональных данных.

**Безопасные сделки** (`YooKassaDealsClient`): [создание](https://yookassa.ru/developers/api#create_deal), [информация](https://yookassa.ru/developers/api#get_deal), [список](https://yookassa.ru/developers/api#get_deals_list).

**Webhook** (`YooKassaWebhooksClient`): [создание](https://yookassa.ru/developers/api#create_webhook), [список](https://yookassa.ru/developers/api#get_webhook_list), [удаление](https://yookassa.ru/developers/api#delete_webhook); разбор [входящих уведомлений](https://yookassa.ru/developers/using-api/webhooks) через `YooKassaNotification.Parse`.

**Магазин**: [настройки](https://yookassa.ru/developers/api#get_me) через `YooKassaClient.GetMeAsync`.

Пока не покрыто отдельными методами: способы оплаты, счета, чеки и поиск выплат. Чеки 54-ФЗ можно передавать внутри платежей, возвратов и выплат через свойство `Receipt`.

## Лицензия

MIT.

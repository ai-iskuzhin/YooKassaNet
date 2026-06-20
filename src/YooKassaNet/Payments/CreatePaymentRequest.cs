using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Запрос на создание платежа.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_payment">Создание платежа</see>.
/// </remarks>
public sealed record CreatePaymentRequest
{
    /// <summary>Сумма платежа.</summary>
    [JsonPropertyName("amount")]
    public required Money Amount { get; init; }

    /// <summary>Описание платежа (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Данные для чека (54-ФЗ).</summary>
    [JsonPropertyName("receipt")]
    public Receipt? Receipt { get; init; }

    /// <summary>Сценарий подтверждения платежа покупателем.</summary>
    [JsonPropertyName("confirmation")]
    public Confirmation? Confirmation { get; init; }

    /// <summary>Автоматически списать платеж (одностадийный платеж).</summary>
    [JsonPropertyName("capture")]
    public bool? Capture { get; init; }

    /// <summary>Одноразовый токен, полученный из виджета или мобильного SDK.</summary>
    [JsonPropertyName("payment_token")]
    public string? PaymentToken { get; init; }

    /// <summary>Идентификатор сохраненного способа оплаты для автоплатежа.</summary>
    [JsonPropertyName("payment_method_id")]
    public string? PaymentMethodId { get; init; }

    /// <summary>Данные способа оплаты для оплаты конкретным методом.</summary>
    [JsonPropertyName("payment_method_data")]
    public PaymentMethodData? PaymentMethodData { get; init; }

    /// <summary>Сохранить способ оплаты для последующих автоплатежей.</summary>
    [JsonPropertyName("save_payment_method")]
    public bool? SavePaymentMethod { get; init; }

    /// <summary>IP-адрес покупателя.</summary>
    [JsonPropertyName("client_ip")]
    public string? ClientIp { get; init; }

    /// <summary>Идентификатор покупателя в системе магазина.</summary>
    [JsonPropertyName("merchant_customer_id")]
    public string? MerchantCustomerId { get; init; }

    /// <summary>Данные сделки (безопасная сделка).</summary>
    [JsonPropertyName("deal")]
    public PaymentDeal? Deal { get; init; }

    /// <summary>Произвольные метаданные (до 16 ключей).</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

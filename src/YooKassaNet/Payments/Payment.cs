using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Объект платежа ЮKassa.
/// </summary>
/// <remarks>
/// Объект платежа содержит всю информацию о платеже, актуальную на текущий момент.
/// <see href="https://yookassa.ru/developers/api#payment_object">Объект Payment</see>.
/// </remarks>
public sealed record Payment
{
    /// <summary>Идентификатор платежа в ЮKassa.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Статус платежа.</summary>
    [JsonPropertyName("status")]
    public PaymentStatus Status { get; init; }

    /// <summary>Сумма платежа.</summary>
    [JsonPropertyName("amount")]
    public Money Amount { get; init; } = new(0);

    /// <summary>Сумма к зачислению на счет магазина за вычетом комиссии.</summary>
    [JsonPropertyName("income_amount")]
    public Money? IncomeAmount { get; init; }

    /// <summary>Возвращенная сумма.</summary>
    [JsonPropertyName("refunded_amount")]
    public Money? RefundedAmount { get; init; }

    /// <summary>Получены ли деньги.</summary>
    [JsonPropertyName("paid")]
    public bool Paid { get; init; }

    /// <summary>Доступен ли возврат.</summary>
    [JsonPropertyName("refundable")]
    public bool Refundable { get; init; }

    /// <summary>Описание платежа (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Получатель платежа.</summary>
    [JsonPropertyName("recipient")]
    public PaymentRecipient? Recipient { get; init; }

    /// <summary>Способ оплаты.</summary>
    [JsonPropertyName("payment_method")]
    public PaymentMethod? PaymentMethod { get; init; }

    /// <summary>Данные о подтверждении платежа.</summary>
    [JsonPropertyName("confirmation")]
    public Confirmation? Confirmation { get; init; }

    /// <summary>Детали авторизации.</summary>
    [JsonPropertyName("authorization_details")]
    public AuthorizationDetails? AuthorizationDetails { get; init; }

    /// <summary>Детали отмены, если платеж отменен.</summary>
    [JsonPropertyName("cancellation_details")]
    public CancellationDetails? CancellationDetails { get; init; }

    /// <summary>Момент создания платежа.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Момент подтверждения (списания) платежа.</summary>
    [JsonPropertyName("captured_at")]
    public DateTimeOffset? CapturedAt { get; init; }

    /// <summary>Момент, до которого нужно подтвердить платеж.</summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset? ExpiresAt { get; init; }

    /// <summary>Тестовый ли платеж.</summary>
    [JsonPropertyName("test")]
    public bool Test { get; init; }

    /// <summary>Идентификатор сделки, в рамках которой проведен платеж.</summary>
    [JsonPropertyName("merchant_customer_id")]
    public string? MerchantCustomerId { get; init; }

    /// <summary>Произвольные метаданные (до 16 ключей).</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

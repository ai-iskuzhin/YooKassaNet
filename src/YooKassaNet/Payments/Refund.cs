using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Объект возврата ЮKassa.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#refund_object">Объект Refund</see>.
/// </remarks>
public sealed record Refund
{
    /// <summary>Идентификатор возврата.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Статус возврата.</summary>
    [JsonPropertyName("status")]
    public RefundStatus Status { get; init; }

    /// <summary>Идентификатор платежа, по которому сделан возврат.</summary>
    [JsonPropertyName("payment_id")]
    public string PaymentId { get; init; } = string.Empty;

    /// <summary>Сумма возврата.</summary>
    [JsonPropertyName("amount")]
    public Money Amount { get; init; } = new(0);

    /// <summary>Момент создания возврата.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Описание возврата.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Детали авторизации возврата.</summary>
    [JsonPropertyName("refund_authorization_details")]
    public RefundAuthorizationDetails? RefundAuthorizationDetails { get; init; }

    /// <summary>Детали отмены, если возврат отменен.</summary>
    [JsonPropertyName("cancellation_details")]
    public CancellationDetails? CancellationDetails { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

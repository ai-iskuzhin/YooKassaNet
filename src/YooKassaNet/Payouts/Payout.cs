using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Объект выплаты ЮKassa.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#payout_object">Объект Payout</see>.
/// </remarks>
public sealed record Payout
{
    /// <summary>Идентификатор выплаты.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Статус выплаты.</summary>
    [JsonPropertyName("status")]
    public PayoutStatus Status { get; init; }

    /// <summary>Сумма выплаты.</summary>
    [JsonPropertyName("amount")]
    public Money Amount { get; init; } = new(0);

    /// <summary>Цель выплаты.</summary>
    [JsonPropertyName("payout_destination")]
    public PayoutDestination? PayoutDestination { get; init; }

    /// <summary>Описание выплаты (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Идентификатор сделки, в рамках которой проведена выплата.</summary>
    [JsonPropertyName("deal")]
    public PayoutDealReference? Deal { get; init; }

    /// <summary>Момент создания выплаты.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Момент успешного завершения выплаты.</summary>
    [JsonPropertyName("succeeded_at")]
    public DateTimeOffset? SucceededAt { get; init; }

    /// <summary>Детали отмены, если выплата отменена.</summary>
    [JsonPropertyName("cancellation_details")]
    public CancellationDetails? CancellationDetails { get; init; }

    /// <summary>Тестовая ли выплата.</summary>
    [JsonPropertyName("test")]
    public bool Test { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

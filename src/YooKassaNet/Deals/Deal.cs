using System.Text.Json.Serialization;

namespace YooKassaNet.Deals;

/// <summary>
/// Объект безопасной сделки ЮKassa.
/// </summary>
/// <remarks>
/// Сделка связывает платеж покупателя и выплату продавцу с удержанием средств на время сделки.
/// <see href="https://yookassa.ru/developers/api#deal_object">Объект Deal</see>.
/// </remarks>
public sealed record Deal
{
    /// <summary>Идентификатор сделки.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Тип сделки.</summary>
    [JsonPropertyName("type")]
    public DealType Type { get; init; }

    /// <summary>Момент списания комиссии.</summary>
    [JsonPropertyName("fee_moment")]
    public FeeMoment FeeMoment { get; init; }

    /// <summary>Статус сделки.</summary>
    [JsonPropertyName("status")]
    public DealStatus Status { get; init; }

    /// <summary>Текущий баланс сделки.</summary>
    [JsonPropertyName("balance")]
    public Money? Balance { get; init; }

    /// <summary>Сумма, доступная для выплаты по сделке.</summary>
    [JsonPropertyName("payout_balance")]
    public Money? PayoutBalance { get; init; }

    /// <summary>Описание сделки (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Момент создания сделки.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Момент, после которого сделка истекает.</summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset? ExpiresAt { get; init; }

    /// <summary>Тестовая ли сделка.</summary>
    [JsonPropertyName("test")]
    public bool Test { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

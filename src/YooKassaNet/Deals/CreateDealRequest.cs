using System.Text.Json.Serialization;

namespace YooKassaNet.Deals;

/// <summary>
/// Запрос на создание безопасной сделки.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_deal">Создание сделки</see>.
/// </remarks>
public sealed record CreateDealRequest
{
    /// <summary>Тип сделки.</summary>
    [JsonPropertyName("type")]
    public DealType Type { get; init; } = DealType.SafeDeal;

    /// <summary>Момент списания комиссии.</summary>
    [JsonPropertyName("fee_moment")]
    public required FeeMoment FeeMoment { get; init; }

    /// <summary>Описание сделки (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

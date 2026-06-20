using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Ссылка на сделку, передаваемая при создании выплаты.
/// </summary>
public sealed record PayoutDeal
{
    /// <summary>Идентификатор сделки.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }
}

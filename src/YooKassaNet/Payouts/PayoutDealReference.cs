using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Ссылка на сделку в объекте выплаты.
/// </summary>
public sealed record PayoutDealReference
{
    /// <summary>Идентификатор сделки.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

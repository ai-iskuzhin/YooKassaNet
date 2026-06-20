using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Детали отмены платежа или выплаты: кто и почему отменил операцию.
/// </summary>
public sealed record CancellationDetails
{
    /// <summary>Участник, инициировавший отмену.</summary>
    [JsonPropertyName("party")]
    public CancellationParty Party { get; init; }

    /// <summary>Причина отмены.</summary>
    [JsonPropertyName("reason")]
    public CancellationReason Reason { get; init; }
}

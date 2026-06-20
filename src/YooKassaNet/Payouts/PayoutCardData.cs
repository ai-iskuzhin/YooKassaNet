using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Данные карты для выплаты на банковскую карту.
/// </summary>
public sealed record PayoutCardData
{
    /// <summary>Номер банковской карты получателя.</summary>
    [JsonPropertyName("number")]
    public required string Number { get; init; }
}

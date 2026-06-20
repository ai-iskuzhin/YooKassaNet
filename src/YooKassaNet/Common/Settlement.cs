using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Расчет в чеке (54-ФЗ).
/// </summary>
public sealed record Settlement
{
    /// <summary>Тип расчета.</summary>
    [JsonPropertyName("type")]
    public SettlementType Type { get; init; }

    /// <summary>Сумма расчета.</summary>
    [JsonPropertyName("amount")]
    public Money? Amount { get; init; }
}

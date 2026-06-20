using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Распределение средств по сделке (расчет в безопасной сделке).
/// </summary>
public sealed record PaymentSettlement
{
    /// <summary>Тип расчета. Для платежей в сделке — <c>payout</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "payout";

    /// <summary>Сумма расчета.</summary>
    [JsonPropertyName("amount")]
    public Money? Amount { get; init; }
}

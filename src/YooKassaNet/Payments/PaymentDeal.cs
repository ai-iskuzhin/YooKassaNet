using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Данные сделки, передаваемые при создании или подтверждении платежа.
/// </summary>
public sealed record PaymentDeal
{
    /// <summary>Идентификатор сделки.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Распределение денег покупателя по сделке.</summary>
    [JsonPropertyName("settlements")]
    public IReadOnlyList<PaymentSettlement>? Settlements { get; init; }
}

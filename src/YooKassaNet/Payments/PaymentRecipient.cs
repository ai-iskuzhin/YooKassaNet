using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Получатель платежа: магазин и шлюз.
/// </summary>
public sealed record PaymentRecipient
{
    /// <summary>Идентификатор магазина.</summary>
    [JsonPropertyName("account_id")]
    public string? AccountId { get; init; }

    /// <summary>Идентификатор субаккаунта (шлюза).</summary>
    [JsonPropertyName("gateway_id")]
    public string? GatewayId { get; init; }
}

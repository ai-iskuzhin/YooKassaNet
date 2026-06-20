using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Детали авторизации возврата.
/// </summary>
public sealed record RefundAuthorizationDetails
{
    /// <summary>Retrieval Reference Number операции возврата.</summary>
    [JsonPropertyName("rrn")]
    public string? Rrn { get; init; }
}

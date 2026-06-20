using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Детали авторизации платежа.
/// </summary>
public sealed record AuthorizationDetails
{
    /// <summary>Retrieval Reference Number операции.</summary>
    [JsonPropertyName("rrn")]
    public string? Rrn { get; init; }

    /// <summary>Код авторизации операции.</summary>
    [JsonPropertyName("auth_code")]
    public string? AuthCode { get; init; }

    /// <summary>Результат проверки 3-D Secure.</summary>
    [JsonPropertyName("three_d_secure")]
    public ThreeDSecureDetails? ThreeDSecure { get; init; }
}

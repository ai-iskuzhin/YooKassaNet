using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Результат проверки 3-D Secure.
/// </summary>
public sealed record ThreeDSecureDetails
{
    /// <summary>Была ли применена аутентификация 3-D Secure.</summary>
    [JsonPropertyName("applied")]
    public bool Applied { get; init; }
}

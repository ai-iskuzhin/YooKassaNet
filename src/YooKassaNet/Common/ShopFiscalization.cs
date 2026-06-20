using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Настройки фискализации магазина.
/// </summary>
public sealed record ShopFiscalization
{
    /// <summary>Провайдер фискализации, например <c>atol</c>.</summary>
    [JsonPropertyName("provider")]
    public string? Provider { get; init; }

    /// <summary>Включена ли фискализация.</summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }
}

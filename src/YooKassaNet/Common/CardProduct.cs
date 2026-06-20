using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Кобейджинговый продукт банковской карты.
/// </summary>
public sealed record CardProduct
{
    /// <summary>Код продукта карты, например <c>MCP</c>.</summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>Название продукта карты, например <c>MIR Privilege</c>.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

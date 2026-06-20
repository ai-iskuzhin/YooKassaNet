using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Данные самозанятого получателя выплаты.
/// </summary>
public sealed record SelfEmployed
{
    /// <summary>Идентификатор самозанятого в ЮKassa.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }
}

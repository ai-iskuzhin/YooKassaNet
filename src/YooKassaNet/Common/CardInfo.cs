using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Данные банковской карты, возвращаемые ЮKassa (в платеже или выплате).
/// </summary>
/// <remarks>
/// Платежная система карты (<see cref="CardType"/>) оставлена строкой намеренно: это отображаемое
/// название бренда (например <c>Mir</c>, <c>Visa</c>), а не перечисление протокола.
/// </remarks>
public sealed record CardInfo
{
    /// <summary>Первые 6 цифр номера карты.</summary>
    [JsonPropertyName("first6")]
    public string? First6 { get; init; }

    /// <summary>Последние 4 цифры номера карты.</summary>
    [JsonPropertyName("last4")]
    public string? Last4 { get; init; }

    /// <summary>Месяц окончания срока действия в формате <c>MM</c>.</summary>
    [JsonPropertyName("expiry_month")]
    public string? ExpiryMonth { get; init; }

    /// <summary>Год окончания срока действия в формате <c>YYYY</c>.</summary>
    [JsonPropertyName("expiry_year")]
    public string? ExpiryYear { get; init; }

    /// <summary>Платежная система карты, например <c>Mir</c> или <c>Visa</c>.</summary>
    [JsonPropertyName("card_type")]
    public string? CardType { get; init; }

    /// <summary>Кобейджинговый продукт карты, если применимо.</summary>
    [JsonPropertyName("card_product")]
    public CardProduct? CardProduct { get; init; }

    /// <summary>Код страны банка-эмитента в формате ISO 3166-1 alpha-2.</summary>
    [JsonPropertyName("issuer_country")]
    public string? IssuerCountry { get; init; }

    /// <summary>Название банка-эмитента.</summary>
    [JsonPropertyName("issuer_name")]
    public string? IssuerName { get; init; }
}

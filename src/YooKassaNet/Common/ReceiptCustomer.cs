using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Данные покупателя для чека (54-ФЗ).
/// </summary>
public sealed record ReceiptCustomer
{
    /// <summary>ФИО или наименование организации.</summary>
    [JsonPropertyName("full_name")]
    public string? FullName { get; init; }

    /// <summary>ИНН покупателя.</summary>
    [JsonPropertyName("inn")]
    public string? Inn { get; init; }

    /// <summary>Электронная почта покупателя.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>Телефон покупателя в формате ITU-T E.164.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}

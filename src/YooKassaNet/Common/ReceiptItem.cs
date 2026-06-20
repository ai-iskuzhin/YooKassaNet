using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Позиция чека (54-ФЗ).
/// </summary>
public sealed record ReceiptItem
{
    /// <summary>Наименование товара или услуги.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Количество.</summary>
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; init; }

    /// <summary>Цена за единицу с учетом скидок.</summary>
    [JsonPropertyName("amount")]
    public Money? Amount { get; init; }

    /// <summary>Ставка НДС: код от 1 до 6.</summary>
    [JsonPropertyName("vat_code")]
    public int VatCode { get; init; }

    /// <summary>Признак способа расчета.</summary>
    [JsonPropertyName("payment_mode")]
    public PaymentMode? PaymentMode { get; init; }

    /// <summary>Признак предмета расчета.</summary>
    [JsonPropertyName("payment_subject")]
    public PaymentSubject? PaymentSubject { get; init; }

    /// <summary>Код страны происхождения товара (ISO 3166-1 alpha-2).</summary>
    [JsonPropertyName("country_of_origin_code")]
    public string? CountryOfOriginCode { get; init; }

    /// <summary>Номер таможенной декларации.</summary>
    [JsonPropertyName("customs_declaration_number")]
    public string? CustomsDeclarationNumber { get; init; }

    /// <summary>Сумма акциза.</summary>
    [JsonPropertyName("excise")]
    public string? Excise { get; init; }
}

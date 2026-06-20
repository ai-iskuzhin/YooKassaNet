using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Данные для формирования чека (54-ФЗ), передаваемые при создании платежа, возврата или выплаты.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/payment-acceptance/receipts/54fz/other-services/basics">Чеки 54-ФЗ</see>.
/// </remarks>
public sealed record Receipt
{
    /// <summary>Данные покупателя.</summary>
    [JsonPropertyName("customer")]
    public ReceiptCustomer? Customer { get; init; }

    /// <summary>Позиции чека.</summary>
    [JsonPropertyName("items")]
    public IReadOnlyList<ReceiptItem> Items { get; init; } = Array.Empty<ReceiptItem>();

    /// <summary>Система налогообложения (1–6), если их несколько.</summary>
    [JsonPropertyName("tax_system_code")]
    public int? TaxSystemCode { get; init; }

    /// <summary>Электронная почта для отправки чека.</summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>Телефон для отправки чека.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}

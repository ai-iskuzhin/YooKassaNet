using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Запрос на создание возврата.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_refund">Создание возврата</see>.
/// </remarks>
public sealed record CreateRefundRequest
{
    /// <summary>Идентификатор платежа для возврата.</summary>
    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; init; }

    /// <summary>Сумма возврата.</summary>
    [JsonPropertyName("amount")]
    public required Money Amount { get; init; }

    /// <summary>Описание возврата (до 250 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Данные для чека возврата (54-ФЗ).</summary>
    [JsonPropertyName("receipt")]
    public Receipt? Receipt { get; init; }

    /// <summary>Данные сделки для возврата.</summary>
    [JsonPropertyName("deal")]
    public PaymentDeal? Deal { get; init; }
}

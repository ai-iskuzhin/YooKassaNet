using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Запрос на подтверждение (списание) ранее авторизованного платежа.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#capture_payment">Подтверждение платежа</see>.
/// Сумма может быть меньше авторизованной; неиспользованный остаток вернется покупателю.
/// </remarks>
public sealed record CapturePaymentRequest
{
    /// <summary>Сумма к списанию. Если не указана, списывается вся авторизованная сумма.</summary>
    [JsonPropertyName("amount")]
    public Money? Amount { get; init; }

    /// <summary>Данные для чека (54-ФЗ).</summary>
    [JsonPropertyName("receipt")]
    public Receipt? Receipt { get; init; }

    /// <summary>Данные сделки.</summary>
    [JsonPropertyName("deal")]
    public PaymentDeal? Deal { get; init; }
}

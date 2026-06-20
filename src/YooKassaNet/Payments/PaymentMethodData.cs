using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Данные способа оплаты, передаваемые при создании платежа.
/// </summary>
/// <remarks>
/// Минимально достаточно указать <see cref="Type"/>; для ряда способов нужны дополнительные поля.
/// </remarks>
public sealed record PaymentMethodData
{
    /// <summary>Тип способа оплаты.</summary>
    [JsonPropertyName("type")]
    public PaymentMethodType Type { get; init; }

    /// <summary>Телефон плательщика для способов, которым он требуется (например <c>mobile_balance</c>).</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}

using System.Text.Json.Serialization;

namespace YooKassaNet.Payments;

/// <summary>
/// Способ оплаты, привязанный к платежу (объект ответа).
/// </summary>
public sealed record PaymentMethod
{
    /// <summary>Тип способа оплаты.</summary>
    [JsonPropertyName("type")]
    public PaymentMethodType Type { get; init; }

    /// <summary>Идентификатор способа оплаты.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Сохранен ли способ оплаты для автоплатежей.</summary>
    [JsonPropertyName("saved")]
    public bool Saved { get; init; }

    /// <summary>Название способа оплаты, например <c>Bank card *4444</c>.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>Данные банковской карты, если применимо.</summary>
    [JsonPropertyName("card")]
    public CardInfo? Card { get; init; }
}

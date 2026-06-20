using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Запрос на создание выплаты.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_payout">Создание выплаты</see>.
/// </remarks>
public sealed record CreatePayoutRequest
{
    /// <summary>Сумма выплаты.</summary>
    [JsonPropertyName("amount")]
    public required Money Amount { get; init; }

    /// <summary>Данные цели выплаты.</summary>
    [JsonPropertyName("payout_destination_data")]
    public PayoutDestinationData? PayoutDestinationData { get; init; }

    /// <summary>Токен синхронизированной карты, полученный из виджета выплат.</summary>
    [JsonPropertyName("payout_token")]
    public string? PayoutToken { get; init; }

    /// <summary>Идентификатор сохраненного способа оплаты, на который проводится выплата.</summary>
    [JsonPropertyName("payment_method_id")]
    public string? PaymentMethodId { get; init; }

    /// <summary>Описание выплаты (до 128 символов).</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Ссылка на сделку, в рамках которой проводится выплата.</summary>
    [JsonPropertyName("deal")]
    public PayoutDeal? Deal { get; init; }

    /// <summary>Данные самозанятого получателя.</summary>
    [JsonPropertyName("self_employed")]
    public SelfEmployed? SelfEmployed { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

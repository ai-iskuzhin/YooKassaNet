using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Данные цели выплаты, передаваемые при создании выплаты.
/// </summary>
/// <remarks>
/// Укажите либо <see cref="Type"/> с соответствующими полями, либо используйте
/// <see cref="CreatePayoutRequest.PayoutToken"/> или <see cref="CreatePayoutRequest.PaymentMethodId"/>.
/// </remarks>
public sealed record PayoutDestinationData
{
    /// <summary>Тип цели выплаты.</summary>
    [JsonPropertyName("type")]
    public PayoutDestinationType Type { get; init; }

    /// <summary>Данные карты для выплаты на банковскую карту.</summary>
    [JsonPropertyName("card")]
    public PayoutCardData? Card { get; init; }

    /// <summary>Номер кошелька ЮMoney для выплаты на кошелек.</summary>
    [JsonPropertyName("account_number")]
    public string? AccountNumber { get; init; }

    /// <summary>Идентификатор банка-участника СБП для выплаты через СБП.</summary>
    [JsonPropertyName("bank_id")]
    public string? BankId { get; init; }

    /// <summary>Номер телефона получателя в СБП.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}

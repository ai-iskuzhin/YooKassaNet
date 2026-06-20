using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Цель выплаты (объект ответа).
/// </summary>
public sealed record PayoutDestination
{
    /// <summary>Тип цели выплаты.</summary>
    [JsonPropertyName("type")]
    public PayoutDestinationType Type { get; init; }

    /// <summary>Данные карты, если выплата на банковскую карту.</summary>
    [JsonPropertyName("card")]
    public CardInfo? Card { get; init; }

    /// <summary>Номер кошелька ЮMoney, если выплата на кошелек.</summary>
    [JsonPropertyName("account_number")]
    public string? AccountNumber { get; init; }

    /// <summary>Идентификатор банка-участника СБП, если выплата через СБП.</summary>
    [JsonPropertyName("bank_id")]
    public string? BankId { get; init; }

    /// <summary>Номер телефона получателя в СБП.</summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}

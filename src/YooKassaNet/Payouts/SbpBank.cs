using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Банк-участник Системы быстрых платежей (СБП).
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#sbp_bank_object">Объект участника СБП</see>.
/// </remarks>
public sealed record SbpBank
{
    /// <summary>Идентификатор банка в СБП.</summary>
    [JsonPropertyName("bank_id")]
    public string BankId { get; init; } = string.Empty;

    /// <summary>Название банка.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>БИК банка.</summary>
    [JsonPropertyName("bic")]
    public string? Bic { get; init; }
}

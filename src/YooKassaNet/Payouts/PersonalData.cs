using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Объект персональных данных получателя выплаты.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#personal_data_object">Объект персональных данных</see>.
/// </remarks>
public sealed record PersonalData
{
    /// <summary>Идентификатор персональных данных.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Назначение персональных данных.</summary>
    [JsonPropertyName("type")]
    public PersonalDataType Type { get; init; }

    /// <summary>Статус персональных данных.</summary>
    [JsonPropertyName("status")]
    public PersonalDataStatus Status { get; init; }

    /// <summary>Момент создания.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Момент, после которого данные станут недоступны.</summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset? ExpiresAt { get; init; }

    /// <summary>Детали отмены, если применимо.</summary>
    [JsonPropertyName("cancellation_details")]
    public CancellationDetails? CancellationDetails { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

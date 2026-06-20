using System.Text.Json.Serialization;

namespace YooKassaNet.Payouts;

/// <summary>
/// Запрос на создание персональных данных получателя выплаты.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_personal_data">Создание персональных данных</see>.
/// </remarks>
public sealed record CreatePersonalDataRequest
{
    /// <summary>Назначение персональных данных.</summary>
    [JsonPropertyName("type")]
    public required PersonalDataType Type { get; init; }

    /// <summary>Фамилия получателя.</summary>
    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    /// <summary>Имя получателя.</summary>
    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    /// <summary>Отчество получателя.</summary>
    [JsonPropertyName("middle_name")]
    public string? MiddleName { get; init; }

    /// <summary>Дата рождения получателя в формате <c>YYYY-MM-DD</c>.</summary>
    [JsonPropertyName("birthdate")]
    public string? Birthdate { get; init; }

    /// <summary>Произвольные метаданные.</summary>
    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

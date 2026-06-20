using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Объект ошибки ЮKassa, возвращаемый при HTTP-статусах вне диапазона 2xx.
/// </summary>
/// <remarks>
/// Описание ошибок: <see href="https://yookassa.ru/developers/using-api/interaction-format#error-object">формат ошибок</see>.
/// Код ошибки оставлен строкой намеренно: новый код не должен приводить к ошибке разбора при обработке уже неуспешного ответа.
/// </remarks>
public sealed record YooKassaError
{
    /// <summary>Тип объекта, всегда <c>error</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Идентификатор ошибки для обращения в поддержку.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Код ошибки, например <c>invalid_request</c> или <c>not_supported</c>.</summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>Человекочитаемое описание ошибки.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Имя параметра запроса, вызвавшего ошибку, если применимо.</summary>
    [JsonPropertyName("parameter")]
    public string? Parameter { get; init; }

    /// <summary>Через сколько секунд можно повторить запрос при ошибке <c>too_many_requests</c>.</summary>
    [JsonPropertyName("retry_after")]
    public int? RetryAfter { get; init; }
}

using System.Text.Json.Serialization;

namespace YooKassaNet.Webhooks;

/// <summary>
/// Зарегистрированный webhook.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#webhook_object">Объект Webhook</see>.
/// </remarks>
public sealed record Webhook
{
    /// <summary>Идентификатор webhook.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Событие, на которое подписан webhook.</summary>
    [JsonPropertyName("event")]
    public WebhookEvent Event { get; init; }

    /// <summary>URL, на который ЮKassa отправляет уведомления.</summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;
}

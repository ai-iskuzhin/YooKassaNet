using System.Text.Json.Serialization;

namespace YooKassaNet.Webhooks;

/// <summary>
/// Запрос на создание webhook.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#create_webhook">Создание webhook</see>.
/// </remarks>
public sealed record CreateWebhookRequest
{
    /// <summary>Событие, на которое нужно подписаться.</summary>
    [JsonPropertyName("event")]
    public required WebhookEvent Event { get; init; }

    /// <summary>URL, на который будут приходить уведомления (HTTPS).</summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}

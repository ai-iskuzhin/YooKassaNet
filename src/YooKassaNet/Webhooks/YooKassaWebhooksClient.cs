namespace YooKassaNet.Webhooks;

/// <summary>
/// Клиент управления webhook-подписками ЮKassa.
/// </summary>
/// <remarks>
/// Управление webhook доступно при аутентификации по OAuth-токену.
/// <see href="https://yookassa.ru/developers/using-api/webhooks">Уведомления</see>.
/// </remarks>
public sealed class YooKassaWebhooksClient
{
    private readonly YooKassaApiClient api;

    /// <summary>
    /// Создает клиент webhook ЮKassa.
    /// </summary>
    /// <param name="httpClient">HTTP-клиент для отправки запросов.</param>
    /// <param name="options">Настройки аутентификации и адреса API.</param>
    public YooKassaWebhooksClient(HttpClient httpClient, YooKassaClientOptions options)
    {
        api = new YooKassaApiClient(httpClient, options);
    }

    /// <summary>
    /// Создает webhook-подписку на событие.
    /// </summary>
    /// <param name="request">Событие и URL уведомлений.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданный <see cref="Webhook"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_webhook">Создание webhook</see>.</remarks>
    public Task<Webhook> CreateWebhookAsync(
        CreateWebhookRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<Webhook>("webhooks", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает список созданных webhook.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список webhook.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_webhook_list">Список webhook</see>.</remarks>
    public Task<YooKassaList<Webhook>> GetWebhooksAsync(CancellationToken cancellationToken = default)
    {
        return api.GetAsync<YooKassaList<Webhook>>("webhooks", cancellationToken);
    }

    /// <summary>
    /// Удаляет webhook-подписку.
    /// </summary>
    /// <param name="webhookId">Идентификатор webhook.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, завершающаяся после удаления.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#delete_webhook">Удаление webhook</see>.</remarks>
    public Task DeleteWebhookAsync(string webhookId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(webhookId))
        {
            throw new YooKassaValidationException($"Параметр '{nameof(webhookId)}' обязателен.");
        }

        return api.DeleteAsync($"webhooks/{Uri.EscapeDataString(webhookId)}", cancellationToken);
    }
}

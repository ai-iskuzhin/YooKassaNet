namespace YooKassaNet.Deals;

/// <summary>
/// Клиент API безопасных сделок ЮKassa.
/// </summary>
/// <remarks>
/// Сделки используются вместе с платежами и выплатами в сценарии «Безопасная сделка».
/// Аутентификация: учетные данные магазина-платформы.
/// <see href="https://yookassa.ru/developers/solutions-for-platforms/safe-deal/basics">Безопасная сделка</see>.
/// </remarks>
public sealed class YooKassaDealsClient
{
    private readonly YooKassaApiClient api;

    /// <summary>
    /// Создает клиент сделок ЮKassa.
    /// </summary>
    /// <param name="httpClient">HTTP-клиент для отправки запросов.</param>
    /// <param name="options">Настройки аутентификации и адреса API.</param>
    public YooKassaDealsClient(HttpClient httpClient, YooKassaClientOptions options)
    {
        api = new YooKassaApiClient(httpClient, options);
    }

    /// <summary>
    /// Создает безопасную сделку.
    /// </summary>
    /// <param name="request">Параметры сделки.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданная <see cref="Deal"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_deal">Создание сделки</see>.</remarks>
    public Task<Deal> CreateDealAsync(
        CreateDealRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<Deal>("deals", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает информацию о сделке.
    /// </summary>
    /// <param name="dealId">Идентификатор сделки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="Deal"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_deal">Информация о сделке</see>.</remarks>
    public Task<Deal> GetDealAsync(string dealId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dealId))
        {
            throw new YooKassaValidationException($"Параметр '{nameof(dealId)}' обязателен.");
        }

        return api.GetAsync<Deal>($"deals/{Uri.EscapeDataString(dealId)}", cancellationToken);
    }

    /// <summary>
    /// Возвращает список сделок.
    /// </summary>
    /// <param name="filter">Фильтр и параметры постраничного вывода.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Страница списка сделок.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_deals_list">Список сделок</see>.</remarks>
    public Task<YooKassaList<Deal>> GetDealsAsync(
        DealListFilter? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = filter?.ToQueryString() ?? string.Empty;
        return api.GetAsync<YooKassaList<Deal>>("deals" + query, cancellationToken);
    }
}

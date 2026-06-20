namespace YooKassaNet.Payouts;

/// <summary>
/// Клиент API выплат ЮKassa: выплаты, участники СБП и персональные данные.
/// </summary>
/// <remarks>
/// Аутентификация: идентификатор шлюза выплат (агента) + секретный ключ выплат — это, как правило,
/// отдельные учетные данные, не совпадающие с ключами для приема платежей.
/// <see href="https://yookassa.ru/developers/payouts/making-payouts/api-basics">Основы работы с выплатами</see>.
/// </remarks>
/// <example>
/// <code>
/// using var http = new HttpClient();
/// var payouts = new YooKassaPayoutsClient(http, new YooKassaClientOptions
/// {
///     ShopId = "513961",       // идентификатор шлюза выплат
///     SecretKey = "test_...",  // ключ выплат
/// });
/// </code>
/// </example>
public sealed class YooKassaPayoutsClient
{
    private readonly YooKassaApiClient api;

    /// <summary>
    /// Создает клиент выплат ЮKassa.
    /// </summary>
    /// <param name="httpClient">HTTP-клиент для отправки запросов.</param>
    /// <param name="options">Настройки аутентификации шлюза выплат и адреса API.</param>
    public YooKassaPayoutsClient(HttpClient httpClient, YooKassaClientOptions options)
    {
        api = new YooKassaApiClient(httpClient, options);
    }

    /// <summary>
    /// Создает выплату.
    /// </summary>
    /// <param name="request">Параметры выплаты.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданная <see cref="Payout"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_payout">Создание выплаты</see>.</remarks>
    public Task<Payout> CreatePayoutAsync(
        CreatePayoutRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<Payout>("payouts", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает информацию о выплате.
    /// </summary>
    /// <param name="payoutId">Идентификатор выплаты.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="Payout"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_payout">Информация о выплате</see>.</remarks>
    public Task<Payout> GetPayoutAsync(string payoutId, CancellationToken cancellationToken = default)
    {
        RequireId(payoutId, nameof(payoutId));
        return api.GetAsync<Payout>($"payouts/{Uri.EscapeDataString(payoutId)}", cancellationToken);
    }

    /// <summary>
    /// Возвращает список банков-участников СБП, доступных для выплат.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список участников СБП.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_sbp_banks_list">Список участников СБП</see>.</remarks>
    public Task<YooKassaList<SbpBank>> GetSbpBanksAsync(CancellationToken cancellationToken = default)
    {
        return api.GetAsync<YooKassaList<SbpBank>>("sbp_banks", cancellationToken);
    }

    /// <summary>
    /// Создает персональные данные получателя выплаты.
    /// </summary>
    /// <param name="request">Параметры персональных данных.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданный объект <see cref="PersonalData"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_personal_data">Создание персональных данных</see>.</remarks>
    public Task<PersonalData> CreatePersonalDataAsync(
        CreatePersonalDataRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<PersonalData>("personal_data", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает информацию о персональных данных.
    /// </summary>
    /// <param name="personalDataId">Идентификатор персональных данных.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="PersonalData"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_personal_data">Информация о персональных данных</see>.</remarks>
    public Task<PersonalData> GetPersonalDataAsync(string personalDataId, CancellationToken cancellationToken = default)
    {
        RequireId(personalDataId, nameof(personalDataId));
        return api.GetAsync<PersonalData>($"personal_data/{Uri.EscapeDataString(personalDataId)}", cancellationToken);
    }

    private static void RequireId(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new YooKassaValidationException($"Параметр '{paramName}' обязателен.");
        }
    }
}

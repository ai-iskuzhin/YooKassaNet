namespace YooKassaNet.Payments;

/// <summary>
/// Клиент API платежей и возвратов ЮKassa.
/// </summary>
/// <remarks>
/// Аутентификация: <c>shopId</c> + секретный ключ магазина. Базовый справочник методов:
/// <see href="https://yookassa.ru/developers/api">API ЮKassa</see>.
/// </remarks>
/// <example>
/// <code>
/// using var http = new HttpClient();
/// var payments = new YooKassaPaymentsClient(http, new YooKassaClientOptions
/// {
///     ShopId = "1281498",
///     SecretKey = "test_...",
/// });
///
/// var payment = await payments.CreatePaymentAsync(new CreatePaymentRequest
/// {
///     Amount = Money.Rubles(100m),
///     Capture = true,
///     Confirmation = Confirmation.Redirect("https://example.com/return"),
///     Description = "Заказ №37",
/// });
/// </code>
/// </example>
public sealed class YooKassaPaymentsClient
{
    private readonly YooKassaApiClient api;

    /// <summary>
    /// Создает клиент платежей ЮKassa.
    /// </summary>
    /// <param name="httpClient">HTTP-клиент для отправки запросов.</param>
    /// <param name="options">Настройки аутентификации и адреса API.</param>
    public YooKassaPaymentsClient(HttpClient httpClient, YooKassaClientOptions options)
    {
        api = new YooKassaApiClient(httpClient, options);
    }

    /// <summary>
    /// Создает платеж.
    /// </summary>
    /// <param name="request">Параметры создаваемого платежа.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданный <see cref="Payment"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_payment">Создание платежа</see>.</remarks>
    public Task<Payment> CreatePaymentAsync(
        CreatePaymentRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<Payment>("payments", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает информацию о платеже.
    /// </summary>
    /// <param name="paymentId">Идентификатор платежа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="Payment"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_payment">Информация о платеже</see>.</remarks>
    public Task<Payment> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        RequireId(paymentId, nameof(paymentId));
        return api.GetAsync<Payment>($"payments/{Uri.EscapeDataString(paymentId)}", cancellationToken);
    }

    /// <summary>
    /// Возвращает список платежей.
    /// </summary>
    /// <param name="filter">Фильтр и параметры постраничного вывода.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Страница списка платежей.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_payments_list">Список платежей</see>.</remarks>
    public Task<YooKassaList<Payment>> GetPaymentsAsync(
        PaymentListFilter? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = filter?.ToQueryString() ?? string.Empty;
        return api.GetAsync<YooKassaList<Payment>>("payments" + query, cancellationToken);
    }

    /// <summary>
    /// Подтверждает (списывает) ранее авторизованный платеж в статусе <see cref="PaymentStatus.WaitingForCapture"/>.
    /// </summary>
    /// <param name="paymentId">Идентификатор платежа.</param>
    /// <param name="request">Параметры подтверждения. Если не указаны, списывается вся авторизованная сумма.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный <see cref="Payment"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#capture_payment">Подтверждение платежа</see>.</remarks>
    public Task<Payment> CapturePaymentAsync(
        string paymentId,
        CapturePaymentRequest? request = null,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        RequireId(paymentId, nameof(paymentId));
        return api.PostAsync<Payment>(
            $"payments/{Uri.EscapeDataString(paymentId)}/capture",
            request ?? new CapturePaymentRequest(),
            idempotenceKey,
            cancellationToken);
    }

    /// <summary>
    /// Отменяет платеж в статусе <see cref="PaymentStatus.WaitingForCapture"/> и возвращает деньги покупателю.
    /// </summary>
    /// <param name="paymentId">Идентификатор платежа.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный <see cref="Payment"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#cancel_payment">Отмена платежа</see>.</remarks>
    public Task<Payment> CancelPaymentAsync(
        string paymentId,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        RequireId(paymentId, nameof(paymentId));
        return api.PostAsync<Payment>(
            $"payments/{Uri.EscapeDataString(paymentId)}/cancel",
            body: new object(),
            idempotenceKey,
            cancellationToken);
    }

    /// <summary>
    /// Создает возврат по платежу.
    /// </summary>
    /// <param name="request">Параметры возврата.</param>
    /// <param name="idempotenceKey">Ключ идемпотентности. Если не указан, генерируется автоматически.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданный <see cref="Refund"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#create_refund">Создание возврата</see>.</remarks>
    public Task<Refund> CreateRefundAsync(
        CreateRefundRequest request,
        string? idempotenceKey = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return api.PostAsync<Refund>("refunds", request, idempotenceKey, cancellationToken);
    }

    /// <summary>
    /// Возвращает информацию о возврате.
    /// </summary>
    /// <param name="refundId">Идентификатор возврата.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="Refund"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_refund">Информация о возврате</see>.</remarks>
    public Task<Refund> GetRefundAsync(string refundId, CancellationToken cancellationToken = default)
    {
        RequireId(refundId, nameof(refundId));
        return api.GetAsync<Refund>($"refunds/{Uri.EscapeDataString(refundId)}", cancellationToken);
    }

    /// <summary>
    /// Возвращает список возвратов.
    /// </summary>
    /// <param name="filter">Фильтр и параметры постраничного вывода.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Страница списка возвратов.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_refunds_list">Список возвратов</see>.</remarks>
    public Task<YooKassaList<Refund>> GetRefundsAsync(
        RefundListFilter? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = filter?.ToQueryString() ?? string.Empty;
        return api.GetAsync<YooKassaList<Refund>>("refunds" + query, cancellationToken);
    }

    private static void RequireId(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new YooKassaValidationException($"Параметр '{paramName}' обязателен.");
        }
    }
}

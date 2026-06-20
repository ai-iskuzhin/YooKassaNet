using YooKassaNet.Deals;
using YooKassaNet.Payments;
using YooKassaNet.Payouts;
using YooKassaNet.Webhooks;

namespace YooKassaNet;

/// <summary>
/// Единая точка доступа ко всем областям API ЮKassa: платежи, выплаты, сделки и webhook.
/// </summary>
/// <remarks>
/// Платежи, сделки и webhook используют учетные данные магазина, а выплаты, как правило, требуют
/// отдельных учетных данных шлюза выплат — передайте их вторым параметром конструктора. Если нужна
/// только одна область, можно создавать конкретный клиент напрямую
/// (<see cref="YooKassaPaymentsClient"/>, <see cref="YooKassaPayoutsClient"/>, <see cref="YooKassaDealsClient"/>).
/// </remarks>
/// <example>
/// <code>
/// using var http = new HttpClient();
/// var yoo = new YooKassaClient(
///     http,
///     new YooKassaClientOptions { ShopId = "1281498", SecretKey = "test_..." },     // платежи/сделки
///     new YooKassaClientOptions { ShopId = "513961", SecretKey = "test_..." });     // выплаты
///
/// var payment = await yoo.Payments.CreatePaymentAsync(new CreatePaymentRequest
/// {
///     Amount = Money.Rubles(100m),
///     Capture = true,
///     Confirmation = Confirmation.Redirect("https://example.com/return"),
/// });
/// </code>
/// </example>
public sealed class YooKassaClient
{
    private readonly YooKassaApiClient api;

    /// <summary>
    /// Создает фасад клиентов ЮKassa.
    /// </summary>
    /// <param name="httpClient">HTTP-клиент для отправки запросов.</param>
    /// <param name="options">Учетные данные магазина для платежей, сделок и webhook.</param>
    /// <param name="payoutOptions">Учетные данные шлюза выплат. Если не указаны, используются <paramref name="options"/>.</param>
    public YooKassaClient(
        HttpClient httpClient,
        YooKassaClientOptions options,
        YooKassaClientOptions? payoutOptions = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        api = new YooKassaApiClient(httpClient, options);
        Payments = new YooKassaPaymentsClient(httpClient, options);
        Deals = new YooKassaDealsClient(httpClient, options);
        Webhooks = new YooKassaWebhooksClient(httpClient, options);
        Payouts = new YooKassaPayoutsClient(httpClient, payoutOptions ?? options);
    }

    /// <summary>Клиент платежей и возвратов.</summary>
    public YooKassaPaymentsClient Payments { get; }

    /// <summary>Клиент выплат, участников СБП и персональных данных.</summary>
    public YooKassaPayoutsClient Payouts { get; }

    /// <summary>Клиент безопасных сделок.</summary>
    public YooKassaDealsClient Deals { get; }

    /// <summary>Клиент управления webhook-подписками.</summary>
    public YooKassaWebhooksClient Webhooks { get; }

    /// <summary>
    /// Возвращает информацию о настройках магазина или шлюза.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Объект <see cref="ShopInfo"/>.</returns>
    /// <remarks><see href="https://yookassa.ru/developers/api#get_me">Информация о настройках</see>.</remarks>
    public Task<ShopInfo> GetMeAsync(CancellationToken cancellationToken = default)
    {
        return api.GetAsync<ShopInfo>("me", cancellationToken);
    }
}

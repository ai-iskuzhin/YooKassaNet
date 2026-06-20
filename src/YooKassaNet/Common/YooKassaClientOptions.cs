using System.Text;

namespace YooKassaNet;

/// <summary>
/// Настройки клиентов ЮKassa: аутентификация и адрес API.
/// </summary>
/// <remarks>
/// Аутентификация возможна двумя способами: HTTP Basic по паре идентификатор+секретный ключ
/// (для платежей это <c>shopId</c>, для выплат — идентификатор шлюза/агента) либо по OAuth-токену.
/// <see href="https://yookassa.ru/developers/using-api/interaction-format#auth">Аутентификация</see>.
/// </remarks>
/// <example>
/// <code>
/// // Платежи: shopId + секретный ключ.
/// var options = new YooKassaClientOptions { ShopId = "1281498", SecretKey = "test_..." };
///
/// // Выплаты: идентификатор шлюза выплат + ключ выплат.
/// var payoutOptions = new YooKassaClientOptions { ShopId = "513961", SecretKey = "test_..." };
/// </code>
/// </example>
public sealed class YooKassaClientOptions
{
    /// <summary>Идентификатор магазина (<c>shopId</c>) или шлюза выплат для Basic-аутентификации.</summary>
    public string? ShopId { get; init; }

    /// <summary>Секретный ключ для Basic-аутентификации.</summary>
    public string? SecretKey { get; init; }

    /// <summary>OAuth-токен для Bearer-аутентификации вместо пары <see cref="ShopId"/>/<see cref="SecretKey"/>.</summary>
    public string? OAuthToken { get; init; }

    /// <summary>Базовый адрес API. По умолчанию <c>https://api.yookassa.ru/v3/</c>.</summary>
    public Uri BaseAddress { get; init; } = new Uri("https://api.yookassa.ru/v3/");

    /// <summary>Включать полное необрезанное тело ответа в <see cref="YooKassaApiException.ResponseBodyPreview"/> для диагностики.</summary>
    /// <value><see langword="false"/> по умолчанию: фрагмент тела обрезается, чтобы случайно не хранить чувствительные данные.</value>
    public bool CaptureRawResponseBody { get; init; }

    internal void Validate()
    {
        if (!string.IsNullOrWhiteSpace(OAuthToken))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ShopId) || string.IsNullOrWhiteSpace(SecretKey))
        {
            throw new ArgumentException(
                "Configure either OAuthToken or both ShopId and SecretKey.",
                nameof(YooKassaClientOptions));
        }
    }

    internal string BuildAuthorizationHeader()
    {
        if (!string.IsNullOrWhiteSpace(OAuthToken))
        {
            return "Bearer " + OAuthToken;
        }

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ShopId}:{SecretKey}"));
        return "Basic " + credentials;
    }
}

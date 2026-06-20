using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace YooKassaNet;

/// <summary>
/// Внутренний HTTP-транспорт ЮKassa: аутентификация, ключ идемпотентности, сериализация и разбор ошибок.
/// </summary>
/// <remarks>
/// Общая основа всех типизированных клиентов (платежи, выплаты, сделки). Не предназначен для прямого
/// использования: создавайте <see cref="Payments.YooKassaPaymentsClient"/>, <see cref="Payouts.YooKassaPayoutsClient"/>,
/// <see cref="Deals.YooKassaDealsClient"/> или фасад <see cref="YooKassaClient"/>.
/// </remarks>
internal sealed class YooKassaApiClient
{
    private static readonly string UserAgent = BuildUserAgent();

    private readonly HttpClient httpClient;
    private readonly YooKassaClientOptions options;

    public YooKassaApiClient(HttpClient httpClient, YooKassaClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        this.httpClient = httpClient;
        this.options = options;
    }

    public Task<T> GetAsync<T>(string path, CancellationToken cancellationToken)
        => SendAsync<T>(HttpMethod.Get, path, body: null, idempotenceKey: null, cancellationToken);

    public Task<T> PostAsync<T>(string path, object? body, string? idempotenceKey, CancellationToken cancellationToken)
        => SendAsync<T>(HttpMethod.Post, path, body, idempotenceKey, cancellationToken);

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        using var response = await SendCoreAsync(HttpMethod.Delete, path, body: null, idempotenceKey: null, cancellationToken)
            .ConfigureAwait(false);

        var responseBody = await ReadBodyAsync(response, cancellationToken).ConfigureAwait(false);
        EnsureSuccess(response, responseBody, "DELETE " + path);
    }

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string path,
        object? body,
        string? idempotenceKey,
        CancellationToken cancellationToken)
    {
        using var response = await SendCoreAsync(method, path, body, idempotenceKey, cancellationToken)
            .ConfigureAwait(false);

        var responseBody = await ReadBodyAsync(response, cancellationToken).ConfigureAwait(false);
        var operation = $"{method.Method} {path}";
        EnsureSuccess(response, responseBody, operation);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            throw new YooKassaProtocolException(
                $"YooKassa {operation} вернул пустое тело ответа. HTTP {(int)response.StatusCode} ({response.StatusCode}).",
                response.StatusCode);
        }

        try
        {
            return JsonSerializer.Deserialize<T>(responseBody, YooKassaJson.Options)
                ?? throw new YooKassaProtocolException(
                    $"YooKassa {operation} вернул пустой объект после десериализации.",
                    response.StatusCode,
                    CreateBodyPreview(responseBody));
        }
        catch (JsonException exception)
        {
            throw new YooKassaProtocolException(
                $"YooKassa {operation}: тело ответа не является ожидаемым JSON. HTTP {(int)response.StatusCode} ({response.StatusCode}).",
                response.StatusCode,
                CreateBodyPreview(responseBody),
                exception);
        }
    }

    private async Task<HttpResponseMessage> SendCoreAsync(
        HttpMethod method,
        string path,
        object? body,
        string? idempotenceKey,
        CancellationToken cancellationToken)
    {
        var endpoint = new Uri(options.BaseAddress, path);

        try
        {
            using var request = new HttpRequestMessage(method, endpoint);
            request.Headers.TryAddWithoutValidation("Authorization", options.BuildAuthorizationHeader());
            request.Headers.UserAgent.ParseAdd(UserAgent);

            // Идемпотентность обязательна для POST: ЮKassa дедуплицирует повторы по этому ключу.
            if (method == HttpMethod.Post)
            {
                request.Headers.TryAddWithoutValidation(
                    "Idempotence-Key",
                    string.IsNullOrWhiteSpace(idempotenceKey) ? Guid.NewGuid().ToString() : idempotenceKey);
            }

            if (body is not null)
            {
                request.Content = JsonContent.Create(body, body.GetType(), options: YooKassaJson.Options);
            }

            return await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException exception)
        {
            throw new YooKassaTransportException(
                $"Запрос YooKassa {method.Method} {path} завершился ошибкой до получения ответа.",
                exception);
        }
    }

    private void EnsureSuccess(HttpResponseMessage response, string responseBody, string operation)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        YooKassaError? error = null;
        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            try
            {
                error = JsonSerializer.Deserialize<YooKassaError>(responseBody, YooKassaJson.Options);
            }
            catch (JsonException)
            {
                // Тело не является объектом ошибки ЮKassa; оставляем error == null.
            }
        }

        var description = error?.Description ?? error?.Code ?? response.ReasonPhrase;
        throw new YooKassaApiException(
            $"YooKassa {operation} вернул HTTP {(int)response.StatusCode} ({response.StatusCode}): {description}",
            response.StatusCode,
            error,
            options.CaptureRawResponseBody ? responseBody : CreateBodyPreview(responseBody));
    }

    private static async Task<string> ReadBodyAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#if NETSTANDARD2_0
        _ = cancellationToken;
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#else
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#endif
    }

    private static string CreateBodyPreview(string responseBody)
    {
        var preview = RedactSensitiveFields(responseBody);
        const int maxLength = 512;
        return preview.Length <= maxLength ? preview : preview.Substring(0, maxLength);
    }

    private static string RedactSensitiveFields(string value)
    {
        return Regex.Replace(
            value,
            "(\"(?:number|csc|cvv|cvc|expiry_year|expiry_month|payment_token)\"\\s*:\\s*\")([^\"]*)(\")",
            "$1***REDACTED***$3",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private static string BuildUserAgent()
    {
        var assembly = typeof(YooKassaApiClient).Assembly;
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? assembly.GetName().Version?.ToString()
            ?? "0.0.0";

        var metadataSeparator = version.IndexOf('+');
        if (metadataSeparator >= 0)
        {
            version = version.Substring(0, metadataSeparator);
        }

        return $"YooKassaNet/{version} ({RuntimeInformation.FrameworkDescription})";
    }
}

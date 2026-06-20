using System.Net;

namespace YooKassaNet.Tests;

/// <summary>
/// Записывает последний запрос и возвращает заранее заданный ответ. Для офлайн-тестов клиентов.
/// </summary>
internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode statusCode;
    private readonly string responseBody;

    public StubHttpMessageHandler(HttpStatusCode statusCode, string responseBody)
    {
        this.statusCode = statusCode;
        this.responseBody = responseBody;
    }

    public HttpRequestMessage? LastRequest { get; private set; }

    public string? LastRequestBody { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        LastRequestBody = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "application/json"),
        };
    }
}

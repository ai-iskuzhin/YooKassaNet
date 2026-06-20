using System.Net;
using System.Text;
using YooKassaNet.Payments;

namespace YooKassaNet.Tests;

public sealed class PaymentsClientTests
{
    private const string CreatePaymentResponse = """
        {
          "id": "22e12f66-000f-5000-8000-18db351245c7",
          "status": "pending",
          "paid": false,
          "amount": { "value": "100.00", "currency": "RUB" },
          "confirmation": {
            "type": "redirect",
            "return_url": "https://example.com/return",
            "confirmation_url": "https://yoomoney.ru/checkout/payments/v2/contract?orderId=22e12f66"
          },
          "created_at": "2018-07-18T10:51:18.139Z",
          "description": "Order #37",
          "metadata": { "order_id": "37" },
          "recipient": { "account_id": "100500", "gateway_id": "100700" },
          "refundable": false,
          "test": false
        }
        """;

    private static (YooKassaPaymentsClient Client, StubHttpMessageHandler Handler) CreateClient(
        HttpStatusCode status = HttpStatusCode.OK,
        string body = CreatePaymentResponse)
    {
        var handler = new StubHttpMessageHandler(status, body);
        var http = new HttpClient(handler);
        var client = new YooKassaPaymentsClient(http, new YooKassaClientOptions
        {
            ShopId = "1281498",
            SecretKey = "test_secret",
        });
        return (client, handler);
    }

    [Fact]
    public async Task CreatePaymentAsync_SendsAuthenticatedIdempotentPost()
    {
        var (client, handler) = CreateClient();

        var payment = await client.CreatePaymentAsync(new CreatePaymentRequest
        {
            Amount = Money.Rubles(100m),
            Capture = true,
            Confirmation = Confirmation.Redirect("https://example.com/return"),
            Description = "Order #37",
        });

        var request = handler.LastRequest!;
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://api.yookassa.ru/v3/payments", request.RequestUri!.ToString());

        var expectedAuth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("1281498:test_secret"));
        Assert.Equal(expectedAuth, request.Headers.GetValues("Authorization").Single());
        Assert.True(request.Headers.Contains("Idempotence-Key"));
        Assert.False(string.IsNullOrWhiteSpace(request.Headers.GetValues("Idempotence-Key").Single()));

        Assert.Contains("\"value\":\"100.00\"", handler.LastRequestBody);
        Assert.Contains("\"capture\":true", handler.LastRequestBody);

        Assert.Equal("22e12f66-000f-5000-8000-18db351245c7", payment.Id);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
        Assert.Equal(ConfirmationType.Redirect, payment.Confirmation!.Type);
        Assert.Equal("37", payment.Metadata!["order_id"]);
    }

    [Fact]
    public async Task CreatePaymentAsync_UsesSuppliedIdempotenceKey()
    {
        var (client, handler) = CreateClient();

        await client.CreatePaymentAsync(
            new CreatePaymentRequest { Amount = Money.Rubles(100m) },
            idempotenceKey: "order-37-create");

        Assert.Equal("order-37-create", handler.LastRequest!.Headers.GetValues("Idempotence-Key").Single());
    }

    [Fact]
    public async Task GetPaymentAsync_SendsGetToPaymentPath()
    {
        var (client, handler) = CreateClient();

        await client.GetPaymentAsync("22e12f66-000f-5000-8000-18db351245c7");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Equal(
            "https://api.yookassa.ru/v3/payments/22e12f66-000f-5000-8000-18db351245c7",
            handler.LastRequest.RequestUri!.ToString());
        Assert.False(handler.LastRequest.Headers.Contains("Idempotence-Key"));
    }

    [Fact]
    public async Task CancelPaymentAsync_PostsToCancelPath()
    {
        var (client, handler) = CreateClient();

        await client.CancelPaymentAsync("22e12f66-000f-5000-8000-18db351245c7");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.EndsWith("/payments/22e12f66-000f-5000-8000-18db351245c7/cancel", handler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetPaymentsAsync_ParsesListWithCursor()
    {
        const string listBody = """
            {
              "type": "list",
              "items": [
                {
                  "id": "22e12f66-000f-5000-8000-18db351245c7",
                  "status": "succeeded",
                  "paid": true,
                  "amount": { "value": "2.00", "currency": "RUB" },
                  "created_at": "2018-07-18T10:51:18.139Z",
                  "refundable": true,
                  "test": false
                }
              ],
              "next_cursor": "37a5c87d-3984-51e8-a7f3-8de646d39ec15"
            }
            """;
        var (client, _) = CreateClient(body: listBody);

        var page = await client.GetPaymentsAsync(new PaymentListFilter { Limit = 1, Status = PaymentStatus.Succeeded });

        Assert.Single(page.Items);
        Assert.True(page.HasMore);
        Assert.Equal("37a5c87d-3984-51e8-a7f3-8de646d39ec15", page.NextCursor);
        Assert.Equal(PaymentStatus.Succeeded, page.Items[0].Status);
    }

    [Fact]
    public async Task GetPaymentsAsync_BuildsQueryString()
    {
        const string listBody = """{ "type": "list", "items": [] }""";
        var (client, handler) = CreateClient(body: listBody);

        await client.GetPaymentsAsync(new PaymentListFilter { Limit = 50, Status = PaymentStatus.WaitingForCapture });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("limit=50", uri);
        Assert.Contains("status=waiting_for_capture", uri);
    }

    [Fact]
    public async Task CreatePaymentAsync_OnApiError_ThrowsWithParsedError()
    {
        const string errorBody = """
            {
              "type": "error",
              "id": "ab5a11cd-13f8-5000-8000-1d5d9c2a5b21",
              "code": "invalid_request",
              "description": "Idempotence key duplicated",
              "parameter": "Idempotence-Key"
            }
            """;
        var (client, _) = CreateClient(HttpStatusCode.BadRequest, errorBody);

        var exception = await Assert.ThrowsAsync<YooKassaApiException>(
            () => client.CreatePaymentAsync(new CreatePaymentRequest { Amount = Money.Rubles(100m) }));

        Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
        Assert.Equal("invalid_request", exception.Error!.Code);
        Assert.Equal("Idempotence-Key", exception.Error.Parameter);
    }

    [Fact]
    public async Task GetPaymentAsync_WithBlankId_ThrowsValidation()
    {
        var (client, _) = CreateClient();

        await Assert.ThrowsAsync<YooKassaValidationException>(() => client.GetPaymentAsync("  "));
    }
}

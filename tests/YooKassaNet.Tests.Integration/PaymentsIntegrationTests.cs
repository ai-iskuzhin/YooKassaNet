using YooKassaNet.Payments;

namespace YooKassaNet.Tests.Integration;

public sealed class PaymentsIntegrationTests
{
    [Fact]
    public async Task GetMe_ReturnsAccount_WhenShopCredentialsConfigured()
    {
        if (!IntegrationConfig.HasShopCredentials)
        {
            return;
        }

        using var http = new HttpClient();
        var client = new YooKassaClient(http, IntegrationConfig.ShopOptions());

        var me = await client.GetMeAsync();

        Assert.False(string.IsNullOrWhiteSpace(me.AccountId));
    }

    [Fact]
    public async Task CreateAndGetPayment_RoundTrips_WhenShopCredentialsConfigured()
    {
        if (!IntegrationConfig.HasShopCredentials)
        {
            return;
        }

        using var http = new HttpClient();
        var payments = new YooKassaPaymentsClient(http, IntegrationConfig.ShopOptions());

        var created = await payments.CreatePaymentAsync(new CreatePaymentRequest
        {
            Amount = Money.Rubles(10m),
            Capture = false,
            Confirmation = Confirmation.Redirect("https://example.com/return"),
            Description = "YooKassaNet integration test",
            Metadata = new Dictionary<string, string> { ["test"] = "yookassanet" },
        });

        Assert.False(string.IsNullOrWhiteSpace(created.Id));
        Assert.Equal(PaymentStatus.Pending, created.Status);
        Assert.False(string.IsNullOrWhiteSpace(created.Confirmation?.ConfirmationUrl));

        var fetched = await payments.GetPaymentAsync(created.Id);
        Assert.Equal(created.Id, fetched.Id);
        Assert.Equal(10m, fetched.Amount.Value);
    }
}

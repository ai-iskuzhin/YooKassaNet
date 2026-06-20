using YooKassaNet.Deals;

namespace YooKassaNet.Tests.Integration;

public sealed class DealsIntegrationTests
{
    [Fact]
    public async Task CreateAndGetDeal_RoundTrips_WhenShopCredentialsConfigured()
    {
        if (!IntegrationConfig.HasShopCredentials)
        {
            return;
        }

        using var http = new HttpClient();
        var deals = new YooKassaDealsClient(http, IntegrationConfig.ShopOptions());

        Deal created;
        try
        {
            created = await deals.CreateDealAsync(new CreateDealRequest
            {
                FeeMoment = FeeMoment.PaymentSucceeded,
                Description = "YooKassaNet integration test deal",
                Metadata = new Dictionary<string, string> { ["test"] = "yookassanet" },
            });
        }
        catch (YooKassaApiException exception) when (exception.HttpStatusCode is System.Net.HttpStatusCode.Forbidden)
        {
            // Безопасные сделки доступны не каждому тестовому магазину — пропускаем, если метод не разрешен.
            return;
        }

        Assert.False(string.IsNullOrWhiteSpace(created.Id));
        Assert.Equal(DealStatus.Opened, created.Status);

        var fetched = await deals.GetDealAsync(created.Id);
        Assert.Equal(created.Id, fetched.Id);
    }
}

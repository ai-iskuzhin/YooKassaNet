using YooKassaNet.Payouts;

namespace YooKassaNet.Tests.Integration;

public sealed class PayoutsIntegrationTests
{
    [Fact]
    public async Task GetSbpBanks_ReturnsParticipants_WhenPayoutCredentialsConfigured()
    {
        if (!IntegrationConfig.HasPayoutCredentials)
        {
            return;
        }

        using var http = new HttpClient();
        var payouts = new YooKassaPayoutsClient(http, IntegrationConfig.PayoutOptions());

        YooKassaList<SbpBank> banks;
        try
        {
            banks = await payouts.GetSbpBanksAsync();
        }
        catch (YooKassaApiException exception) when (exception.HttpStatusCode is System.Net.HttpStatusCode.Forbidden)
        {
            // Шлюз выплат тестового аккаунта не активирован для выплат/СБП — пропускаем.
            return;
        }

        Assert.All(banks.Items, bank => Assert.False(string.IsNullOrWhiteSpace(bank.BankId)));
    }
}

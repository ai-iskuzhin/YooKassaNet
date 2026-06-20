using System.Text.Json;

namespace YooKassaNet.Tests;

public sealed class MoneyTests
{
    [Fact]
    public void Serialize_WritesValueAsTwoDecimalStringAndCurrency()
    {
        var json = JsonSerializer.Serialize(new Money(2m, Currency.Rub), YooKassaJson.Options);

        Assert.Equal("{\"value\":\"2.00\",\"currency\":\"RUB\"}", json);
    }

    [Fact]
    public void Deserialize_ReadsValueFromStringWithVaryingScale()
    {
        var money = JsonSerializer.Deserialize<Money>("{\"value\":\"1\",\"currency\":\"USD\"}", YooKassaJson.Options);

        Assert.NotNull(money);
        Assert.Equal(1m, money!.Value);
        Assert.Equal(Currency.Usd, money.Currency);
    }

    [Fact]
    public void RoundTrip_PreservesValueAndCurrency()
    {
        var original = new Money(2500.50m, Currency.Eur);

        var json = JsonSerializer.Serialize(original, YooKassaJson.Options);
        var restored = JsonSerializer.Deserialize<Money>(json, YooKassaJson.Options);

        Assert.Equal(original, restored);
    }
}

using System.Text.Json;
using YooKassaNet.Payments;

namespace YooKassaNet.Tests;

public sealed class EnumConverterTests
{
    [Fact]
    public void Read_MapsKnownWireValue()
    {
        var status = JsonSerializer.Deserialize<PaymentStatus>("\"waiting_for_capture\"", YooKassaJson.Options);

        Assert.Equal(PaymentStatus.WaitingForCapture, status);
    }

    [Fact]
    public void Write_EmitsWireValue()
    {
        var json = JsonSerializer.Serialize(PaymentStatus.Succeeded, YooKassaJson.Options);

        Assert.Equal("\"succeeded\"", json);
    }

    [Fact]
    public void Read_UnknownValue_ThrowsProtocolExceptionWithReportLink()
    {
        var exception = Assert.Throws<YooKassaProtocolException>(
            () => JsonSerializer.Deserialize<PaymentStatus>("\"teleported\"", YooKassaJson.Options));

        Assert.Contains("teleported", exception.Message);
        Assert.Contains(YooKassaWireParsing.ReportIssueUrl, exception.Message);
    }
}

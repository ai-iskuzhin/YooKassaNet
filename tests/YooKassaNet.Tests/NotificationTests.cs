using YooKassaNet.Payments;
using YooKassaNet.Webhooks;

namespace YooKassaNet.Tests;

public sealed class NotificationTests
{
    [Fact]
    public void Parse_PaymentSucceeded_ExposesEventAndTypedObject()
    {
        const string body = """
            {
              "type": "notification",
              "event": "payment.succeeded",
              "object": {
                "id": "22e12f66-000f-5000-8000-18db351245c7",
                "status": "succeeded",
                "paid": true,
                "amount": { "value": "2.00", "currency": "RUB" },
                "created_at": "2018-07-18T10:51:18.139Z",
                "refundable": true,
                "test": false
              }
            }
            """;

        var notification = YooKassaNotification.Parse(body);

        Assert.Equal(WebhookEvent.PaymentSucceeded, notification.Event);

        var payment = notification.AsPayment();
        Assert.Equal("22e12f66-000f-5000-8000-18db351245c7", payment.Id);
        Assert.Equal(PaymentStatus.Succeeded, payment.Status);
        Assert.Equal(2.00m, payment.Amount.Value);
    }

    [Fact]
    public void Parse_UnknownEvent_ThrowsProtocolExceptionWithReportLink()
    {
        const string body = """{ "type": "notification", "event": "payment.teleported", "object": {} }""";

        var exception = Assert.Throws<YooKassaProtocolException>(() => YooKassaNotification.Parse(body));

        Assert.Contains(YooKassaWireParsing.ReportIssueUrl, exception.Message);
    }
}

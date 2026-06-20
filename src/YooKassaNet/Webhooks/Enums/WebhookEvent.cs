namespace YooKassaNet.Webhooks;

/// <summary>
/// Событие, на которое подписан webhook и которое приходит в уведомлениях.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/using-api/webhooks#events">События webhook</see>.
/// </remarks>
[YooKassaEnum]
public enum WebhookEvent
{
    /// <summary>Платеж ожидает подтверждения магазином.</summary>
    [YooKassaWireName("payment.waiting_for_capture")]
    PaymentWaitingForCapture,

    /// <summary>Платеж успешно завершен.</summary>
    [YooKassaWireName("payment.succeeded")]
    PaymentSucceeded,

    /// <summary>Платеж отменен.</summary>
    [YooKassaWireName("payment.canceled")]
    PaymentCanceled,

    /// <summary>Возврат успешно проведен.</summary>
    [YooKassaWireName("refund.succeeded")]
    RefundSucceeded,

    /// <summary>Выплата успешно проведена.</summary>
    [YooKassaWireName("payout.succeeded")]
    PayoutSucceeded,

    /// <summary>Выплата отменена.</summary>
    [YooKassaWireName("payout.canceled")]
    PayoutCanceled,

    /// <summary>Сделка закрыта.</summary>
    [YooKassaWireName("deal.closed")]
    DealClosed,
}

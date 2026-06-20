namespace YooKassaNet.Deals;

/// <summary>
/// Момент списания комиссии по сделке.
/// </summary>
[YooKassaEnum]
public enum FeeMoment
{
    /// <summary>Комиссия удерживается при успешном платеже.</summary>
    [YooKassaWireName("payment_succeeded")]
    PaymentSucceeded,

    /// <summary>Комиссия удерживается при закрытии сделки.</summary>
    [YooKassaWireName("deal_closed")]
    DealClosed,
}

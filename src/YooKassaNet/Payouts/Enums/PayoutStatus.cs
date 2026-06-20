namespace YooKassaNet.Payouts;

/// <summary>
/// Статус выплаты.
/// </summary>
[YooKassaEnum]
public enum PayoutStatus
{
    /// <summary>Выплата создана и обрабатывается.</summary>
    [YooKassaWireName("pending")]
    Pending,

    /// <summary>Выплата успешно проведена.</summary>
    [YooKassaWireName("succeeded")]
    Succeeded,

    /// <summary>Выплата отменена.</summary>
    [YooKassaWireName("canceled")]
    Canceled,
}

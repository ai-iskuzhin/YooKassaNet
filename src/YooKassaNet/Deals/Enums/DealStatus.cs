namespace YooKassaNet.Deals;

/// <summary>
/// Статус сделки.
/// </summary>
[YooKassaEnum]
public enum DealStatus
{
    /// <summary>Сделка открыта.</summary>
    [YooKassaWireName("opened")]
    Opened,

    /// <summary>Сделка закрыта.</summary>
    [YooKassaWireName("closed")]
    Closed,
}

namespace YooKassaNet;

/// <summary>
/// Статус магазина или шлюза.
/// </summary>
[YooKassaEnum]
public enum ShopStatus
{
    /// <summary>Магазин включен и может принимать запросы.</summary>
    [YooKassaWireName("enabled")]
    Enabled,

    /// <summary>Магазин выключен.</summary>
    [YooKassaWireName("disabled")]
    Disabled,
}

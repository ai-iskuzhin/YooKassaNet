namespace YooKassaNet.Deals;

/// <summary>
/// Тип сделки.
/// </summary>
[YooKassaEnum]
public enum DealType
{
    /// <summary>Безопасная сделка.</summary>
    [YooKassaWireName("safe_deal")]
    SafeDeal,
}

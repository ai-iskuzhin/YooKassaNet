namespace YooKassaNet.Payments;

/// <summary>
/// Статус возврата.
/// </summary>
[YooKassaEnum]
public enum RefundStatus
{
    /// <summary>Возврат в обработке.</summary>
    [YooKassaWireName("pending")]
    Pending,

    /// <summary>Возврат успешно проведен.</summary>
    [YooKassaWireName("succeeded")]
    Succeeded,

    /// <summary>Возврат отменен.</summary>
    [YooKassaWireName("canceled")]
    Canceled,
}

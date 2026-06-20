namespace YooKassaNet;

/// <summary>
/// Участник процесса, инициировавший отмену платежа или выплаты.
/// </summary>
[YooKassaEnum]
public enum CancellationParty
{
    /// <summary>Отмена со стороны ЮMoney.</summary>
    [YooKassaWireName("yoo_money")]
    YooMoney,

    /// <summary>Отмена со стороны платежной системы.</summary>
    [YooKassaWireName("payment_network")]
    PaymentNetwork,

    /// <summary>Отмена со стороны магазина.</summary>
    [YooKassaWireName("merchant")]
    Merchant,
}

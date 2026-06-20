namespace YooKassaNet.Payouts;

/// <summary>
/// Тип цели выплаты.
/// </summary>
[YooKassaEnum]
public enum PayoutDestinationType
{
    /// <summary>Банковская карта.</summary>
    [YooKassaWireName("bank_card")]
    BankCard,

    /// <summary>Кошелек ЮMoney.</summary>
    [YooKassaWireName("yoo_money")]
    YooMoney,

    /// <summary>Система быстрых платежей (СБП).</summary>
    [YooKassaWireName("sbp")]
    Sbp,
}

namespace YooKassaNet;

/// <summary>
/// Признак способа расчета в чеке (54-ФЗ).
/// </summary>
[YooKassaEnum]
public enum PaymentMode
{
    /// <summary>Полная предоплата.</summary>
    [YooKassaWireName("full_prepayment")]
    FullPrepayment,

    /// <summary>Частичная предоплата.</summary>
    [YooKassaWireName("partial_prepayment")]
    PartialPrepayment,

    /// <summary>Аванс.</summary>
    [YooKassaWireName("advance")]
    Advance,

    /// <summary>Полный расчет.</summary>
    [YooKassaWireName("full_payment")]
    FullPayment,

    /// <summary>Частичный расчет и кредит.</summary>
    [YooKassaWireName("partial_payment")]
    PartialPayment,

    /// <summary>Передача в кредит.</summary>
    [YooKassaWireName("credit")]
    Credit,

    /// <summary>Оплата кредита.</summary>
    [YooKassaWireName("credit_payment")]
    CreditPayment,
}

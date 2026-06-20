namespace YooKassaNet;

/// <summary>
/// Тип расчета в платеже чека (54-ФЗ).
/// </summary>
[YooKassaEnum]
public enum SettlementType
{
    /// <summary>Безналичный расчет.</summary>
    [YooKassaWireName("cashless")]
    Cashless,

    /// <summary>Предоплата (аванс).</summary>
    [YooKassaWireName("prepayment")]
    Prepayment,

    /// <summary>Постоплата (кредит).</summary>
    [YooKassaWireName("postpayment")]
    Postpayment,

    /// <summary>Встречное предоставление.</summary>
    [YooKassaWireName("consideration")]
    Consideration,
}

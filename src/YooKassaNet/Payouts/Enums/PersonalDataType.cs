namespace YooKassaNet.Payouts;

/// <summary>
/// Назначение персональных данных.
/// </summary>
[YooKassaEnum]
public enum PersonalDataType
{
    /// <summary>Получатель выплаты через СБП.</summary>
    [YooKassaWireName("sbp_payout_recipient")]
    SbpPayoutRecipient,

    /// <summary>Получатель выплаты для отчета о выплатах.</summary>
    [YooKassaWireName("payout_statement_recipient")]
    PayoutStatementRecipient,
}

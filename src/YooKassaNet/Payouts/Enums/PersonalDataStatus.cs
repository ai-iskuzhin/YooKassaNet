namespace YooKassaNet.Payouts;

/// <summary>
/// Статус персональных данных.
/// </summary>
[YooKassaEnum]
public enum PersonalDataStatus
{
    /// <summary>Данные приняты в обработку.</summary>
    [YooKassaWireName("waiting_for_operation")]
    WaitingForOperation,

    /// <summary>Данные активны и доступны для использования.</summary>
    [YooKassaWireName("active")]
    Active,

    /// <summary>Срок хранения данных истек.</summary>
    [YooKassaWireName("canceled")]
    Canceled,
}

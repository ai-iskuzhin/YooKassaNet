namespace YooKassaNet;

/// <summary>
/// Сценарий подтверждения платежа покупателем.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/payment-acceptance/getting-started/payment-process#user-confirmation">Подтверждение платежа</see>.
/// </remarks>
[YooKassaEnum]
public enum ConfirmationType
{
    /// <summary>Перенаправление покупателя на страницу ЮKassa.</summary>
    [YooKassaWireName("redirect")]
    Redirect,

    /// <summary>Встраиваемый виджет ЮKassa на странице магазина.</summary>
    [YooKassaWireName("embedded")]
    Embedded,

    /// <summary>QR-код для подтверждения.</summary>
    [YooKassaWireName("qr")]
    Qr,

    /// <summary>Подтверждение во внешней системе.</summary>
    [YooKassaWireName("external")]
    External,

    /// <summary>Подтверждение в мобильном приложении.</summary>
    [YooKassaWireName("mobile_application")]
    MobileApplication,
}

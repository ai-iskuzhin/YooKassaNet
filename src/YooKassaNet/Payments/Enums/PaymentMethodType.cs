namespace YooKassaNet.Payments;

/// <summary>
/// Способ оплаты платежа.
/// </summary>
/// <remarks>
/// Полный список: <see href="https://yookassa.ru/developers/payment-acceptance/integration-scenarios/manual-integration/other/supported-payment-methods">поддерживаемые способы оплаты</see>.
/// </remarks>
[YooKassaEnum]
public enum PaymentMethodType
{
    /// <summary>Банковская карта.</summary>
    [YooKassaWireName("bank_card")]
    BankCard,

    /// <summary>ЮMoney.</summary>
    [YooKassaWireName("yoo_money")]
    YooMoney,

    /// <summary>СберБанк (SberPay).</summary>
    [YooKassaWireName("sberbank")]
    Sberbank,

    /// <summary>Система быстрых платежей (СБП).</summary>
    [YooKassaWireName("sbp")]
    Sbp,

    /// <summary>B2B СберБанк.</summary>
    [YooKassaWireName("b2b_sberbank")]
    B2BSberbank,

    /// <summary>Баланс мобильного телефона.</summary>
    [YooKassaWireName("mobile_balance")]
    MobileBalance,

    /// <summary>Наличные.</summary>
    [YooKassaWireName("cash")]
    Cash,

    /// <summary>Заплатить по частям (рассрочка).</summary>
    [YooKassaWireName("installments")]
    Installments,

    /// <summary>Т-Банк (Тинькофф).</summary>
    [YooKassaWireName("tinkoff_bank")]
    TinkoffBank,

    /// <summary>Кредит от СберБанка.</summary>
    [YooKassaWireName("sber_loan")]
    SberLoan,

    /// <summary>Электронный сертификат (НСПК).</summary>
    [YooKassaWireName("electronic_certificate")]
    ElectronicCertificate,
}

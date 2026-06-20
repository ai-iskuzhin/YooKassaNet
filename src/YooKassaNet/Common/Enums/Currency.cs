namespace YooKassaNet;

/// <summary>
/// Валюта суммы в формате ISO 4217.
/// </summary>
/// <remarks>
/// Расчетная валюта магазина обычно <see cref="Rub"/>. Список валют:
/// <see href="https://yookassa.ru/developers/payment-acceptance/getting-started/payment-process#currency">валюты ЮKassa</see>.
/// </remarks>
[YooKassaEnum]
public enum Currency
{
    /// <summary>Российский рубль.</summary>
    [YooKassaWireName("RUB")]
    Rub,

    /// <summary>Доллар США.</summary>
    [YooKassaWireName("USD")]
    Usd,

    /// <summary>Евро.</summary>
    [YooKassaWireName("EUR")]
    Eur,

    /// <summary>Фунт стерлингов.</summary>
    [YooKassaWireName("GBP")]
    Gbp,

    /// <summary>Китайский юань.</summary>
    [YooKassaWireName("CNY")]
    Cny,

    /// <summary>Казахстанский тенге.</summary>
    [YooKassaWireName("KZT")]
    Kzt,

    /// <summary>Белорусский рубль.</summary>
    [YooKassaWireName("BYN")]
    Byn,

    /// <summary>Украинская гривна.</summary>
    [YooKassaWireName("UAH")]
    Uah,
}

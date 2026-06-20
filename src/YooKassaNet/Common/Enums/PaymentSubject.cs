namespace YooKassaNet;

/// <summary>
/// Признак предмета расчета в чеке (54-ФЗ).
/// </summary>
[YooKassaEnum]
public enum PaymentSubject
{
    /// <summary>Товар.</summary>
    [YooKassaWireName("commodity")]
    Commodity,

    /// <summary>Подакцизный товар.</summary>
    [YooKassaWireName("excise")]
    Excise,

    /// <summary>Работа.</summary>
    [YooKassaWireName("job")]
    Job,

    /// <summary>Услуга.</summary>
    [YooKassaWireName("service")]
    Service,

    /// <summary>Ставка в азартной игре.</summary>
    [YooKassaWireName("gambling_bet")]
    GamblingBet,

    /// <summary>Выигрыш в азартной игре.</summary>
    [YooKassaWireName("gambling_prize")]
    GamblingPrize,

    /// <summary>Лотерейный билет.</summary>
    [YooKassaWireName("lottery")]
    Lottery,

    /// <summary>Выигрыш в лотерее.</summary>
    [YooKassaWireName("lottery_prize")]
    LotteryPrize,

    /// <summary>Результат интеллектуальной деятельности.</summary>
    [YooKassaWireName("intellectual_activity")]
    IntellectualActivity,

    /// <summary>Платеж.</summary>
    [YooKassaWireName("payment")]
    Payment,

    /// <summary>Агентское вознаграждение.</summary>
    [YooKassaWireName("agent_commission")]
    AgentCommission,

    /// <summary>Имущественное право.</summary>
    [YooKassaWireName("property_right")]
    PropertyRight,

    /// <summary>Внереализационный доход.</summary>
    [YooKassaWireName("non_operating_gain")]
    NonOperatingGain,

    /// <summary>Страховые взносы.</summary>
    [YooKassaWireName("insurance_premium")]
    InsurancePremium,

    /// <summary>Торговый сбор.</summary>
    [YooKassaWireName("sales_tax")]
    SalesTax,

    /// <summary>Курортный сбор.</summary>
    [YooKassaWireName("resort_fee")]
    ResortFee,

    /// <summary>Несколько вариантов.</summary>
    [YooKassaWireName("composite")]
    Composite,

    /// <summary>Подакцизный товар с маркировкой.</summary>
    [YooKassaWireName("marked_excise")]
    MarkedExcise,

    /// <summary>Подакцизный товар без маркировки.</summary>
    [YooKassaWireName("non_marked_excise")]
    NonMarkedExcise,

    /// <summary>Товар с маркировкой.</summary>
    [YooKassaWireName("marked")]
    Marked,

    /// <summary>Товар без маркировки.</summary>
    [YooKassaWireName("non_marked")]
    NonMarked,

    /// <summary>Иной предмет расчета.</summary>
    [YooKassaWireName("another")]
    Another,
}

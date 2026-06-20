namespace YooKassaNet;

/// <summary>
/// Причина отмены платежа или выплаты.
/// </summary>
/// <remarks>
/// Полные перечни: <see href="https://yookassa.ru/developers/payment-acceptance/after-the-payment/declined-payments#cancellation-details-reason">причины отмены платежей</see>
/// и <see href="https://yookassa.ru/developers/payouts/scenario-extensions/recipient-check#cancellation-details">причины отмены выплат</see>.
/// </remarks>
[YooKassaEnum]
public enum CancellationReason
{
    /// <summary>Не пройдена аутентификация по 3-D Secure.</summary>
    [YooKassaWireName("3d_secure_failed")]
    ThreeDSecureFailed,

    /// <summary>Эмитент отклонил операцию и просит связаться с ним.</summary>
    [YooKassaWireName("call_issuer")]
    CallIssuer,

    /// <summary>Платеж отменен магазином.</summary>
    [YooKassaWireName("canceled_by_merchant")]
    CanceledByMerchant,

    /// <summary>Истек срок действия карты.</summary>
    [YooKassaWireName("card_expired")]
    CardExpired,

    /// <summary>Запрещена оплата картой, выпущенной в этой стране.</summary>
    [YooKassaWireName("country_forbidden")]
    CountryForbidden,

    /// <summary>Истек срок сделки.</summary>
    [YooKassaWireName("deal_expired")]
    DealExpired,

    /// <summary>Истек срок подтверждения платежа.</summary>
    [YooKassaWireName("expired_on_capture")]
    ExpiredOnCapture,

    /// <summary>Истек срок оплаты.</summary>
    [YooKassaWireName("expired_on_confirmation")]
    ExpiredOnConfirmation,

    /// <summary>Операция расценена как мошенническая.</summary>
    [YooKassaWireName("fraud_suspected")]
    FraudSuspected,

    /// <summary>Платеж отклонен по прочим причинам.</summary>
    [YooKassaWireName("general_decline")]
    GeneralDecline,

    /// <summary>Требуется идентификация владельца кошелька.</summary>
    [YooKassaWireName("identification_required")]
    IdentificationRequired,

    /// <summary>Недостаточно средств.</summary>
    [YooKassaWireName("insufficient_funds")]
    InsufficientFunds,

    /// <summary>Внутренний таймаут обработки.</summary>
    [YooKassaWireName("internal_timeout")]
    InternalTimeout,

    /// <summary>Неверный номер карты.</summary>
    [YooKassaWireName("invalid_card_number")]
    InvalidCardNumber,

    /// <summary>Неверный CVC/CVV.</summary>
    [YooKassaWireName("invalid_csc")]
    InvalidCsc,

    /// <summary>Банк-эмитент недоступен.</summary>
    [YooKassaWireName("issuer_unavailable")]
    IssuerUnavailable,

    /// <summary>Превышен лимит платежей для способа оплаты.</summary>
    [YooKassaWireName("payment_method_limit_exceeded")]
    PaymentMethodLimitExceeded,

    /// <summary>Способ оплаты недоступен для этого платежа.</summary>
    [YooKassaWireName("payment_method_restricted")]
    PaymentMethodRestricted,

    /// <summary>Отозвано разрешение на автоплатежи.</summary>
    [YooKassaWireName("permission_revoked")]
    PermissionRevoked,

    /// <summary>Оператор мобильной связи не поддерживается.</summary>
    [YooKassaWireName("unsupported_mobile_operator")]
    UnsupportedMobileOperator,

    /// <summary>Превышен лимит на вывод средств.</summary>
    [YooKassaWireName("withdraw_limit_exceeded")]
    WithdrawLimitExceeded,

    /// <summary>Получатель выплаты не найден.</summary>
    [YooKassaWireName("recipient_not_found")]
    RecipientNotFound,

    /// <summary>Выплаты недоступны для данного магазина/шлюза.</summary>
    [YooKassaWireName("payout_flow_restricted")]
    PayoutFlowRestricted,

    /// <summary>Превышен лимит на одну операцию.</summary>
    [YooKassaWireName("one_time_limit_exceeded")]
    OneTimeLimitExceeded,

    /// <summary>Превышен периодический лимит.</summary>
    [YooKassaWireName("periodic_limit_exceeded")]
    PeriodicLimitExceeded,
}

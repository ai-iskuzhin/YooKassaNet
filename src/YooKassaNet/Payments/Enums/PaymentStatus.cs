namespace YooKassaNet.Payments;

/// <summary>
/// Статус платежа.
/// </summary>
/// <remarks>
/// Жизненный цикл: <see href="https://yookassa.ru/developers/payment-acceptance/getting-started/payment-process#payment-statuses">статусы платежа</see>.
/// </remarks>
[YooKassaEnum]
public enum PaymentStatus
{
    /// <summary>Платеж создан и ожидает действий от пользователя.</summary>
    [YooKassaWireName("pending")]
    Pending,

    /// <summary>Оплата авторизована и ожидает подтверждения магазином (двухстадийный платеж).</summary>
    [YooKassaWireName("waiting_for_capture")]
    WaitingForCapture,

    /// <summary>Платеж успешно завершен.</summary>
    [YooKassaWireName("succeeded")]
    Succeeded,

    /// <summary>Платеж отменен.</summary>
    [YooKassaWireName("canceled")]
    Canceled,
}

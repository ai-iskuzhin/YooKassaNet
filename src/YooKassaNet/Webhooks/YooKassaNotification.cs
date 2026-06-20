using System.Text.Json;
using System.Text.Json.Serialization;
using YooKassaNet.Deals;
using YooKassaNet.Payments;
using YooKassaNet.Payouts;

namespace YooKassaNet.Webhooks;

/// <summary>
/// Входящее уведомление (webhook) от ЮKassa.
/// </summary>
/// <remarks>
/// ЮKassa отправляет уведомления на зарегистрированный URL. Объект внутри уведомления зависит от
/// <see cref="Event"/>: для событий <c>payment.*</c> это платеж, для <c>refund.*</c> — возврат и т.д.
/// Всегда перепроверяйте объект запросом к API, прежде чем выполнять действия.
/// <see href="https://yookassa.ru/developers/using-api/webhooks">Уведомления</see>.
/// </remarks>
public sealed record YooKassaNotification
{
    /// <summary>Тип уведомления, обычно <c>notification</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Событие уведомления.</summary>
    [JsonPropertyName("event")]
    public WebhookEvent Event { get; init; }

    /// <summary>Сырой объект уведомления (платеж, возврат, выплата или сделка).</summary>
    [JsonPropertyName("object")]
    public JsonElement Object { get; init; }

    /// <summary>
    /// Разбирает тело входящего уведомления ЮKassa.
    /// </summary>
    /// <param name="json">Тело HTTP-запроса уведомления.</param>
    /// <returns>Разобранное <see cref="YooKassaNotification"/>.</returns>
    /// <exception cref="YooKassaProtocolException">Если тело не удалось разобрать или событие неизвестно SDK.</exception>
    public static YooKassaNotification Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new YooKassaProtocolException("Тело уведомления ЮKassa пустое.");
        }

        try
        {
            return JsonSerializer.Deserialize<YooKassaNotification>(json, YooKassaJson.Options)
                ?? throw new YooKassaProtocolException("Не удалось разобрать уведомление ЮKassa.");
        }
        catch (JsonException exception)
        {
            throw new YooKassaProtocolException("Тело уведомления ЮKassa не является валидным JSON.", innerException: exception);
        }
    }

    /// <summary>Десериализует объект уведомления как платеж.</summary>
    /// <returns>Объект <see cref="Payment"/>.</returns>
    public Payment AsPayment() => Deserialize<Payment>();

    /// <summary>Десериализует объект уведомления как возврат.</summary>
    /// <returns>Объект <see cref="Refund"/>.</returns>
    public Refund AsRefund() => Deserialize<Refund>();

    /// <summary>Десериализует объект уведомления как выплату.</summary>
    /// <returns>Объект <see cref="Payout"/>.</returns>
    public Payout AsPayout() => Deserialize<Payout>();

    /// <summary>Десериализует объект уведомления как сделку.</summary>
    /// <returns>Объект <see cref="Deal"/>.</returns>
    public Deal AsDeal() => Deserialize<Deal>();

    private T Deserialize<T>()
    {
        try
        {
            return Object.Deserialize<T>(YooKassaJson.Options)
                ?? throw new YooKassaProtocolException($"Объект уведомления ЮKassa пуст при разборе как {typeof(T).Name}.");
        }
        catch (JsonException exception)
        {
            throw new YooKassaProtocolException(
                $"Не удалось разобрать объект уведомления ЮKassa как {typeof(T).Name}.",
                innerException: exception);
        }
    }
}

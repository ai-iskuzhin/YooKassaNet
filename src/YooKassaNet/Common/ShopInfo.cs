using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Информация о настройках магазина или шлюза (ответ метода <c>/me</c>).
/// </summary>
/// <remarks>
/// Список доступных способов оплаты оставлен строками намеренно: набор зависит от настроек магазина и
/// может включать значения, которых нет среди типизированных перечислений SDK.
/// <see href="https://yookassa.ru/developers/api#me_object">Объект настроек</see>.
/// </remarks>
public sealed record ShopInfo
{
    /// <summary>Идентификатор магазина или шлюза.</summary>
    [JsonPropertyName("account_id")]
    public string AccountId { get; init; } = string.Empty;

    /// <summary>Работает ли магазин в тестовом режиме.</summary>
    [JsonPropertyName("test")]
    public bool Test { get; init; }

    /// <summary>Статус магазина.</summary>
    [JsonPropertyName("status")]
    public ShopStatus Status { get; init; }

    /// <summary>Включена ли фискализация (упрощенный признак).</summary>
    [JsonPropertyName("fiscalization_enabled")]
    public bool FiscalizationEnabled { get; init; }

    /// <summary>Подробные настройки фискализации.</summary>
    [JsonPropertyName("fiscalization")]
    public ShopFiscalization? Fiscalization { get; init; }

    /// <summary>Доступные способы оплаты (проводные значения).</summary>
    [JsonPropertyName("payment_methods")]
    public IReadOnlyList<string> PaymentMethods { get; init; } = Array.Empty<string>();
}

using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Данные о подтверждении платежа: передаются в запросе и возвращаются в ответе.
/// </summary>
public sealed record Confirmation
{
    /// <summary>Тип сценария подтверждения.</summary>
    [JsonPropertyName("type")]
    public ConfirmationType Type { get; init; }

    /// <summary>URL, на который вернется покупатель после подтверждения (для <see cref="ConfirmationType.Redirect"/>).</summary>
    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; init; }

    /// <summary>URL, на который нужно перенаправить покупателя (возвращается в ответе).</summary>
    [JsonPropertyName("confirmation_url")]
    public string? ConfirmationUrl { get; init; }

    /// <summary>Токен для инициализации встраиваемого виджета (для <see cref="ConfirmationType.Embedded"/>).</summary>
    [JsonPropertyName("confirmation_token")]
    public string? ConfirmationToken { get; init; }

    /// <summary>Данные QR-кода (для <see cref="ConfirmationType.Qr"/>).</summary>
    [JsonPropertyName("confirmation_data")]
    public string? ConfirmationData { get; init; }

    /// <summary>Язык интерфейса страницы подтверждения, например <c>ru_RU</c>.</summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; init; }

    /// <summary>Запрашивать подтверждение платежа по 3-D Secure принудительно.</summary>
    [JsonPropertyName("enforce")]
    public bool? Enforce { get; init; }

    /// <summary>Создает сценарий подтверждения с перенаправлением.</summary>
    /// <param name="returnUrl">URL возврата покупателя после оплаты.</param>
    /// <returns>Подтверждение типа <see cref="ConfirmationType.Redirect"/>.</returns>
    public static Confirmation Redirect(string returnUrl) => new() { Type = ConfirmationType.Redirect, ReturnUrl = returnUrl };
}

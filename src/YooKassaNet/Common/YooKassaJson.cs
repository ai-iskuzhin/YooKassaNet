using System.Text.Json;
using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Общие настройки сериализации JSON для протокола ЮKassa.
/// </summary>
internal static class YooKassaJson
{
    /// <summary>
    /// Единые настройки: snake_case имена, пропуск null при записи, строковые перечисления ЮKassa.
    /// </summary>
    public static readonly JsonSerializerOptions Options = Create();

    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
        };

        options.Converters.Add(new YooKassaEnumConverterFactory());

        return options;
    }
}

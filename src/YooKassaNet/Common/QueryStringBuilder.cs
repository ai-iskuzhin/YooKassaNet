using System.Globalization;
using System.Text;
using System.Text.Json;

namespace YooKassaNet;

/// <summary>
/// Сборщик строки запроса с URL-кодированием значений для эндпоинтов-списков ЮKassa.
/// </summary>
internal sealed class QueryStringBuilder
{
    private readonly StringBuilder builder = new();

    public void Add(string key, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        Append(key, value!);
    }

    public void Add(string key, int? value)
    {
        if (value is null)
        {
            return;
        }

        Append(key, value.Value.ToString(CultureInfo.InvariantCulture));
    }

    public void Add(string key, DateTimeOffset? value)
    {
        if (value is null)
        {
            return;
        }

        Append(key, value.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
    }

    public void AddEnum<TEnum>(string key, TEnum? value)
        where TEnum : struct, Enum
    {
        if (value is null)
        {
            return;
        }

        var json = JsonSerializer.Serialize(value.Value, YooKassaJson.Options);
        Append(key, json.Trim('"'));
    }

    public string Build() => builder.ToString();

    private void Append(string key, string value)
    {
        builder.Append(builder.Length == 0 ? '?' : '&');
        builder.Append(Uri.EscapeDataString(key));
        builder.Append('=');
        builder.Append(Uri.EscapeDataString(value));
    }
}

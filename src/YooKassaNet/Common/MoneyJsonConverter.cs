using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Конвертер <see cref="Money"/>: значение читается и пишется строкой с двумя знаками после запятой.
/// </summary>
public sealed class MoneyJsonConverter : JsonConverter<Money>
{
    /// <inheritdoc />
    public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Ожидался объект суммы ЮKassa.");
        }

        decimal? value = null;
        Currency? currency = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var property = reader.GetString();
            reader.Read();

            switch (property)
            {
                case "value":
                    var raw = reader.TokenType == JsonTokenType.String ? reader.GetString() : reader.GetDecimal().ToString(CultureInfo.InvariantCulture);
                    if (raw is null || !decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
                    {
                        throw new JsonException($"Не удалось разобрать значение суммы '{raw}'.");
                    }

                    value = parsed;
                    break;
                case "currency":
                    currency = JsonSerializer.Deserialize<Currency>(ref reader, options);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (value is null || currency is null)
        {
            throw new JsonException("Объект суммы ЮKassa должен содержать поля value и currency.");
        }

        return new Money(value.Value, currency.Value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("value", value.Value.ToString("0.00", CultureInfo.InvariantCulture));
        writer.WritePropertyName("currency");
        JsonSerializer.Serialize(writer, value.Currency, options);
        writer.WriteEndObject();
    }
}

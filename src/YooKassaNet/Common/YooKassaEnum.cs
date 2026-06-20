using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Помечает перечисление как часть проводного протокола ЮKassa, сериализуемое строкой через
/// <see cref="YooKassaEnumConverterFactory"/>. Значения на проводе задаются атрибутом
/// <see cref="YooKassaWireNameAttribute"/> на каждом члене.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class YooKassaEnumAttribute : Attribute
{
}

/// <summary>
/// Задает строковое значение члена перечисления в протоколе ЮKassa, например <c>waiting_for_capture</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class YooKassaWireNameAttribute : Attribute
{
    /// <summary>
    /// Создает атрибут с проводным именем члена перечисления.
    /// </summary>
    /// <param name="name">Строковое значение, которое ЮKassa использует в JSON.</param>
    public YooKassaWireNameAttribute(string name)
    {
        Name = name;
    }

    /// <summary>Строковое значение члена перечисления в протоколе ЮKassa.</summary>
    public string Name { get; }
}

/// <summary>
/// Помощник для единообразной обработки неизвестных проводных значений ЮKassa.
/// </summary>
public static class YooKassaWireParsing
{
    /// <summary>
    /// Ссылка для сообщения о пробеле в SDK, когда API ЮKassa вернул неизвестное значение.
    /// </summary>
    public const string ReportIssueUrl = "https://github.com/ai-iskuzhin/YooKassaNet/issues/new";

    /// <summary>
    /// Создает исключение для значения перечисления, которое SDK не знает.
    /// </summary>
    /// <param name="enumDescription">Человекочитаемое описание перечисления, например <c>статус платежа</c>.</param>
    /// <param name="value">Неизвестное значение, полученное от ЮKassa.</param>
    /// <returns>Исключение <see cref="YooKassaProtocolException"/> со ссылкой на трекер задач.</returns>
    public static YooKassaProtocolException UnknownEnumValue(string enumDescription, string? value)
    {
        return new YooKassaProtocolException(
            $"YooKassaNet не распознал значение '{value}' для '{enumDescription}', возвращенное API ЮKassa. " +
            $"Скорее всего, ЮKassa добавила новое значение. Пожалуйста, сообщите об этом: {ReportIssueUrl}",
            responseBodyPreview: value);
    }
}

/// <summary>
/// Фабрика конвертеров для перечислений, помеченных <see cref="YooKassaEnumAttribute"/>.
/// </summary>
/// <remarks>
/// Конвертер читает и пишет строковые значения, заданные <see cref="YooKassaWireNameAttribute"/>,
/// и бросает <see cref="YooKassaProtocolException"/> для значений, которых нет в перечислении.
/// </remarks>
public sealed class YooKassaEnumConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        // Только сам тип перечисления: System.Text.Json сам оборачивает конвертер для Nullable<TEnum>.
        return typeToConvert.IsEnum && typeToConvert.GetCustomAttribute<YooKassaEnumAttribute>() is not null;
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(YooKassaEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

/// <summary>
/// Конвертер одного перечисления ЮKassa между строковыми проводными значениями и членами перечисления.
/// </summary>
/// <typeparam name="TEnum">Тип перечисления, помеченный <see cref="YooKassaEnumAttribute"/>.</typeparam>
public sealed class YooKassaEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly IReadOnlyDictionary<string, TEnum> FromWire = BuildFromWire();
    private static readonly IReadOnlyDictionary<TEnum, string> ToWire = BuildToWire();
    private static readonly string Description = ToHumanReadable(typeof(TEnum).Name);

    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Ожидалась строка для '{Description}', получен {reader.TokenType}.");
        }

        var value = reader.GetString();
        if (value is not null && FromWire.TryGetValue(value, out var result))
        {
            return result;
        }

        throw YooKassaWireParsing.UnknownEnumValue(Description, value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (!ToWire.TryGetValue(value, out var wire))
        {
            throw YooKassaWireParsing.UnknownEnumValue(Description, value.ToString());
        }

        writer.WriteStringValue(wire);
    }

    private static Dictionary<string, TEnum> BuildFromWire()
    {
        var map = new Dictionary<string, TEnum>(StringComparer.Ordinal);
        foreach (var (member, wire) in EnumerateMembers())
        {
            map[wire] = member;
        }

        return map;
    }

    private static Dictionary<TEnum, string> BuildToWire()
    {
        var map = new Dictionary<TEnum, string>();
        foreach (var (member, wire) in EnumerateMembers())
        {
            map[member] = wire;
        }

        return map;
    }

    private static IEnumerable<(TEnum Member, string Wire)> EnumerateMembers()
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var wire = field.GetCustomAttribute<YooKassaWireNameAttribute>()?.Name
                ?? throw new InvalidOperationException(
                    $"Член перечисления '{typeof(TEnum).Name}.{field.Name}' не имеет атрибута [YooKassaWireName].");

            yield return ((TEnum)field.GetValue(null)!, wire);
        }
    }

    private static string ToHumanReadable(string typeName)
    {
        var name = typeName.EndsWith("Status", StringComparison.Ordinal)
            ? typeName.Substring(0, typeName.Length - "Status".Length) + " status"
            : typeName;
        return name;
    }
}

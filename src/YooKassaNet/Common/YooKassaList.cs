using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Постраничный список объектов ЮKassa.
/// </summary>
/// <typeparam name="T">Тип элементов списка.</typeparam>
/// <remarks>
/// Постраничный вывод курсорный: для следующей страницы передайте <see cref="NextCursor"/>
/// в параметр <c>cursor</c>. <see href="https://yookassa.ru/developers/using-api/lists">Работа со списками</see>.
/// </remarks>
public sealed record YooKassaList<T>
{
    /// <summary>Тип ответа, всегда <c>list</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Элементы текущей страницы.</summary>
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>Курсор для следующей страницы или <see langword="null"/>, если страница последняя.</summary>
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }

    /// <summary>Есть ли следующая страница.</summary>
    [JsonIgnore]
    public bool HasMore => !string.IsNullOrEmpty(NextCursor);
}

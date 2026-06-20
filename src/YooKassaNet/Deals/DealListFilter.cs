namespace YooKassaNet.Deals;

/// <summary>
/// Фильтр для получения списка сделок.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#get_deals_list">Список сделок</see>.
/// </remarks>
public sealed record DealListFilter
{
    /// <summary>Максимальное число объектов на странице (1–100).</summary>
    public int? Limit { get; init; }

    /// <summary>Курсор следующей страницы.</summary>
    public string? Cursor { get; init; }

    /// <summary>Фильтр по статусу сделки.</summary>
    public DealStatus? Status { get; init; }

    /// <summary>Сделки, созданные не раньше указанного момента (включительно).</summary>
    public DateTimeOffset? CreatedAtGte { get; init; }

    /// <summary>Сделки, созданные не позже указанного момента (включительно).</summary>
    public DateTimeOffset? CreatedAtLte { get; init; }

    internal string ToQueryString()
    {
        var query = new QueryStringBuilder();
        query.Add("limit", Limit);
        query.Add("cursor", Cursor);
        query.AddEnum("status", Status);
        query.Add("created_at.gte", CreatedAtGte);
        query.Add("created_at.lte", CreatedAtLte);
        return query.Build();
    }
}

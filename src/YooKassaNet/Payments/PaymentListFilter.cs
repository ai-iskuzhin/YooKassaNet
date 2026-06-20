namespace YooKassaNet.Payments;

/// <summary>
/// Фильтр для получения списка платежей.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#get_payments_list">Список платежей</see>.
/// </remarks>
public sealed record PaymentListFilter
{
    /// <summary>Максимальное число объектов на странице (1–100).</summary>
    public int? Limit { get; init; }

    /// <summary>Курсор следующей страницы из <see cref="YooKassaList{T}.NextCursor"/>.</summary>
    public string? Cursor { get; init; }

    /// <summary>Фильтр по статусу платежа.</summary>
    public PaymentStatus? Status { get; init; }

    /// <summary>Платежи, созданные не раньше указанного момента (включительно).</summary>
    public DateTimeOffset? CreatedAtGte { get; init; }

    /// <summary>Платежи, созданные не позже указанного момента (включительно).</summary>
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

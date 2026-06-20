namespace YooKassaNet.Payments;

/// <summary>
/// Фильтр для получения списка возвратов.
/// </summary>
/// <remarks>
/// <see href="https://yookassa.ru/developers/api#get_refunds_list">Список возвратов</see>.
/// </remarks>
public sealed record RefundListFilter
{
    /// <summary>Максимальное число объектов на странице (1–100).</summary>
    public int? Limit { get; init; }

    /// <summary>Курсор следующей страницы.</summary>
    public string? Cursor { get; init; }

    /// <summary>Фильтр по идентификатору платежа.</summary>
    public string? PaymentId { get; init; }

    /// <summary>Фильтр по статусу возврата.</summary>
    public RefundStatus? Status { get; init; }

    /// <summary>Возвраты, созданные не раньше указанного момента (включительно).</summary>
    public DateTimeOffset? CreatedAtGte { get; init; }

    /// <summary>Возвраты, созданные не позже указанного момента (включительно).</summary>
    public DateTimeOffset? CreatedAtLte { get; init; }

    internal string ToQueryString()
    {
        var query = new QueryStringBuilder();
        query.Add("limit", Limit);
        query.Add("cursor", Cursor);
        query.Add("payment_id", PaymentId);
        query.AddEnum("status", Status);
        query.Add("created_at.gte", CreatedAtGte);
        query.Add("created_at.lte", CreatedAtLte);
        return query.Build();
    }
}

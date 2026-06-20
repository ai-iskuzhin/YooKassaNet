using System.Globalization;
using System.Text.Json.Serialization;

namespace YooKassaNet;

/// <summary>
/// Денежная сумма ЮKassa: значение и валюта.
/// </summary>
/// <remarks>
/// На проводе сумма представлена строкой с точкой и двумя знаками после запятой, например
/// <c>{ "value": "2.00", "currency": "RUB" }</c>. SDK хранит значение как <see cref="decimal"/>.
/// </remarks>
[JsonConverter(typeof(MoneyJsonConverter))]
public sealed record Money
{
    /// <summary>
    /// Создает денежную сумму.
    /// </summary>
    /// <param name="value">Значение суммы, например <c>2.00</c>.</param>
    /// <param name="currency">Валюта суммы. По умолчанию <see cref="Currency.Rub"/>.</param>
    public Money(decimal value, Currency currency = Currency.Rub)
    {
        Value = value;
        Currency = currency;
    }

    /// <summary>Значение суммы.</summary>
    public decimal Value { get; init; }

    /// <summary>Валюта суммы.</summary>
    public Currency Currency { get; init; }

    /// <summary>Создает сумму в рублях.</summary>
    /// <param name="value">Значение суммы в рублях.</param>
    /// <returns>Сумма в <see cref="Currency.Rub"/>.</returns>
    public static Money Rubles(decimal value) => new(value, Currency.Rub);

    /// <inheritdoc />
    public override string ToString() => $"{Value.ToString("0.00", CultureInfo.InvariantCulture)} {Currency}";
}

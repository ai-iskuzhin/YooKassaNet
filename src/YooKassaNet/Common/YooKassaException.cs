using System.Net;

namespace YooKassaNet;

/// <summary>
/// Базовое исключение SDK YooKassaNet.
/// </summary>
/// <remarks>
/// Исключения SDK используются для транспортных, протокольных, локальных ошибок и ошибок API ЮKassa.
/// </remarks>
public abstract class YooKassaException : Exception
{
    /// <summary>
    /// Создает исключение SDK.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    protected YooKassaException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Создает исключение SDK с внутренней причиной.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    /// <param name="innerException">Внутренняя причина.</param>
    protected YooKassaException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Ошибка транспорта: запрос не получил корректный HTTP-ответ.
/// </summary>
/// <remarks>Например DNS, TLS, сетевой сбой или ошибка <see cref="HttpClient"/> до получения ответа.</remarks>
public sealed class YooKassaTransportException : YooKassaException
{
    /// <summary>
    /// Создает исключение транспортного уровня.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    /// <param name="innerException">Исходная ошибка транспорта.</param>
    public YooKassaTransportException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Ошибка протокола: ответ получен, но его невозможно безопасно разобрать как ответ ЮKassa.
/// </summary>
/// <remarks>
/// Например пустое тело, JSON неожиданной формы или неизвестное SDK значение перечисления,
/// которое вернул API ЮKassa.
/// </remarks>
public sealed class YooKassaProtocolException : YooKassaException
{
    /// <summary>
    /// Создает исключение протокола ЮKassa.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    /// <param name="httpStatusCode">HTTP-статус ответа, если он был получен.</param>
    /// <param name="responseBodyPreview">Короткий отредактированный фрагмент тела ответа.</param>
    /// <param name="innerException">Исходная ошибка разбора ответа.</param>
    public YooKassaProtocolException(
        string message,
        HttpStatusCode? httpStatusCode = null,
        string? responseBodyPreview = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
        ResponseBodyPreview = responseBodyPreview;
    }

    /// <summary>HTTP-статус ответа, если он был получен.</summary>
    public HttpStatusCode? HttpStatusCode { get; }

    /// <summary>Короткий отредактированный фрагмент тела ответа для диагностики.</summary>
    public string? ResponseBodyPreview { get; }
}

/// <summary>
/// Ошибка локальной валидации запроса до отправки в ЮKassa.
/// </summary>
/// <remarks>Используется для очевидно некорректных запросов: пустой идентификатор, отсутствующая сумма и т.п.</remarks>
public sealed class YooKassaValidationException : YooKassaException
{
    /// <summary>
    /// Создает исключение локальной валидации.
    /// </summary>
    /// <param name="message">Описание ошибки валидации.</param>
    public YooKassaValidationException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Ошибка API ЮKassa: сервис вернул HTTP-ответ с телом ошибки.
/// </summary>
/// <remarks>
/// Возникает при HTTP-статусах вне диапазона 2xx. Свойство <see cref="Error"/> содержит
/// разобранный объект ошибки ЮKassa, когда тело удалось распарсить.
/// </remarks>
public sealed class YooKassaApiException : YooKassaException
{
    /// <summary>
    /// Создает исключение API ЮKassa.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    /// <param name="httpStatusCode">HTTP-статус ответа.</param>
    /// <param name="error">Разобранный объект ошибки ЮKassa, если он доступен.</param>
    /// <param name="responseBodyPreview">Короткий отредактированный фрагмент тела ответа.</param>
    public YooKassaApiException(
        string message,
        HttpStatusCode httpStatusCode,
        YooKassaError? error,
        string? responseBodyPreview)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
        Error = error;
        ResponseBodyPreview = responseBodyPreview;
    }

    /// <summary>HTTP-статус ответа.</summary>
    public HttpStatusCode HttpStatusCode { get; }

    /// <summary>Разобранный объект ошибки ЮKassa, если он доступен.</summary>
    public YooKassaError? Error { get; }

    /// <summary>Короткий отредактированный фрагмент тела ответа для диагностики.</summary>
    public string? ResponseBodyPreview { get; }
}

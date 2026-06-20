namespace YooKassaNet.Tests.Integration;

/// <summary>
/// Загружает учетные данные интеграционных тестов из переменных окружения (и из файла <c>.env</c>,
/// если он есть). Тесты пропускаются, когда учетные данные не заданы.
/// </summary>
internal static class IntegrationConfig
{
    private static readonly object SyncRoot = new();
    private static bool envLoaded;

    public static string? ShopId => Get("YOOKASSA_SHOP_ID");

    public static string? SecretKey => Get("YOOKASSA_SECRET_KEY");

    public static string? PayoutAgentId => Get("YOOKASSA_PAYOUT_AGENT_ID");

    public static string? PayoutSecretKey => Get("YOOKASSA_PAYOUT_SECRET_KEY");

    public static Uri? BaseUrl
    {
        get
        {
            var value = Get("YOOKASSA_BASE_URL");
            return string.IsNullOrWhiteSpace(value) ? null : new Uri(value);
        }
    }

    public static bool HasShopCredentials => !string.IsNullOrWhiteSpace(ShopId) && !string.IsNullOrWhiteSpace(SecretKey);

    public static bool HasPayoutCredentials => !string.IsNullOrWhiteSpace(PayoutAgentId) && !string.IsNullOrWhiteSpace(PayoutSecretKey);

    public static YooKassaClientOptions ShopOptions() => Build(ShopId!, SecretKey!);

    public static YooKassaClientOptions PayoutOptions() => Build(PayoutAgentId!, PayoutSecretKey!);

    private static YooKassaClientOptions Build(string shopId, string secretKey)
    {
        var baseUrl = BaseUrl;
        return baseUrl is null
            ? new YooKassaClientOptions { ShopId = shopId, SecretKey = secretKey }
            : new YooKassaClientOptions { ShopId = shopId, SecretKey = secretKey, BaseAddress = baseUrl };
    }

    private static string? Get(string name)
    {
        EnsureEnvLoaded();
        return Environment.GetEnvironmentVariable(name);
    }

    private static void EnsureEnvLoaded()
    {
        if (envLoaded)
        {
            return;
        }

        lock (SyncRoot)
        {
            if (envLoaded)
            {
                return;
            }

            LoadDotEnv();
            envLoaded = true;
        }
    }

    private static void LoadDotEnv()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, ".env");
            if (File.Exists(candidate))
            {
                ApplyDotEnv(candidate);
                return;
            }

            directory = directory.Parent;
        }
    }

    private static void ApplyDotEnv(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed.StartsWith('#'))
            {
                continue;
            }

            var separator = trimmed.IndexOf('=');
            if (separator <= 0)
            {
                continue;
            }

            var key = trimmed.Substring(0, separator).Trim();
            var value = trimmed.Substring(separator + 1).Trim();

            // Не перезаписываем переменные, уже заданные в окружении (CI имеет приоритет).
            if (Environment.GetEnvironmentVariable(key) is null)
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}

#if NETSTANDARD2_0
using System.Runtime.CompilerServices;

namespace YooKassaNet;

/// <summary>
/// netstandard2.0 polyfill that provides <c>ArgumentNullException.ThrowIfNull</c>. Because it lives
/// in the <c>YooKassaNet</c> namespace, unqualified <c>ArgumentNullException.ThrowIfNull</c> calls in
/// this assembly bind here on netstandard2.0 and to the BCL type on net8.0+.
/// </summary>
internal static class ArgumentNullException
{
    public static void ThrowIfNull(
        object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new System.ArgumentNullException(paramName);
        }
    }
}
#endif

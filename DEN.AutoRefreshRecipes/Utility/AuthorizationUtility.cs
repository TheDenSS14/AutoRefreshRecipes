using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DEN.AutoRefreshRecipes.Utility;

public class AuthorizationUtility
{
    // Stolen from https://github.com/space-wizards/SS14.Watchdog/blob/60bab56ce0e6cfc6ad5a0e6874b80d5d2058e33a/SS14.Watchdog/Utility/AuthorizationUtility.cs
    // and https://github.com/space-wizards/SS14.Watchdog/blob/60bab56ce0e6cfc6ad5a0e6874b80d5d2058e33a/SS14.Watchdog/Utility/Base64Util.cs
    public static bool TryParseBasicAuthentication(string authorization,
        [NotNullWhen(false)] out IActionResult? failure,
        [NotNullWhen(true)] out string? token)
    {
        token = null;

        if (!authorization.StartsWith("Basic "))
        {
            failure = new UnauthorizedResult();
            return false;
        }

        token = Utf8Base64ToString(authorization[6..]);
        failure = null;
        return true;
    }

    /// <summary>
    ///     Shortcut for decoding base64 that contains UTF-8.
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    private static string Utf8Base64ToString(string base64)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }
}
using System.Text.RegularExpressions;

namespace SafeVault.Core.Security;

/// <summary>
/// Provides reusable input validation helpers that can be shared across
/// API endpoints and background jobs.
/// </summary>
public class InputValidator
{
    private static readonly Regex EmailRegex = new(
        pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        options: RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

    /// <summary>
    /// Validates that the supplied email address matches a conservative
    /// RFC 5322 compliant pattern and does not contain whitespace.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns><c>true</c> when the email is considered valid; otherwise <c>false</c>.</returns>
    public bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return EmailRegex.IsMatch(email);
    }
}

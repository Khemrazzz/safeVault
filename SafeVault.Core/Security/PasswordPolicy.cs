namespace SafeVault.Core.Security;

/// <summary>
/// Enforces a baseline password policy that aligns with OWASP recommendations
/// by requiring a mix of character types and a minimum length.
/// </summary>
public class PasswordPolicy
{
    public const int MinimumLength = 12;

    /// <summary>
    /// Determines whether the provided password satisfies the strong password policy.
    /// </summary>
    /// <param name="password">The password to evaluate.</param>
    public bool IsStrongPassword(string? password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < MinimumLength)
        {
            return false;
        }

        var hasUpper = false;
        var hasLower = false;
        var hasDigit = false;
        var hasSpecial = false;

        foreach (var character in password)
        {
            if (char.IsWhiteSpace(character))
            {
                // Reject passwords containing whitespace to avoid user confusion
                // and accidental truncation in HTML forms.
                return false;
            }

            if (char.IsUpper(character))
            {
                hasUpper = true;
            }
            else if (char.IsLower(character))
            {
                hasLower = true;
            }
            else if (char.IsDigit(character))
            {
                hasDigit = true;
            }
            else
            {
                hasSpecial = true;
            }
        }

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}

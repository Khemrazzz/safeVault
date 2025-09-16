using System.Text.Encodings.Web;

namespace SafeVault.Core.Security;

/// <summary>
/// Provides utilities for sanitising user generated content before rendering it
/// in HTML contexts to mitigate cross-site scripting (XSS) attacks.
/// </summary>
public class CommentSanitizer
{
    private readonly HtmlEncoder _htmlEncoder;

    public CommentSanitizer(HtmlEncoder? htmlEncoder = null)
    {
        _htmlEncoder = htmlEncoder ?? HtmlEncoder.Default;
    }

    /// <summary>
    /// HTML encodes arbitrary user supplied text.
    /// </summary>
    public string Encode(string? userSuppliedContent)
    {
        return _htmlEncoder.Encode(userSuppliedContent ?? string.Empty);
    }
}

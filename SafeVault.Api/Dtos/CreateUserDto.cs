using System.ComponentModel.DataAnnotations;
using SafeVault.Core.Security;

namespace SafeVault.Api.Dtos;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(PasswordPolicy.MinimumLength)]
    public string Password { get; set; } = string.Empty;
}

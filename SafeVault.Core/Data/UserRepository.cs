using System.Data;
using System.Data.Common;
using Dapper;
using SafeVault.Core.Models;

namespace SafeVault.Core.Data;

/// <summary>
/// Provides strongly typed data access methods that default to
/// parameterised SQL queries to avoid injection vulnerabilities.
/// </summary>
public class UserRepository
{
    private readonly Func<DbConnection> _connectionFactory;

    public UserRepository(Func<DbConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    /// <summary>
    /// Retrieves a user record by their email address using a parameterised query.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>A matching <see cref="User"/> or <c>null</c> when no record exists.</returns>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must be provided.", nameof(email));
        }

        await using var connection = _connectionFactory();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        const string query = """
            SELECT Id, Email, PasswordHash, Role
            FROM Users
            WHERE Email = @Email
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
    }
}

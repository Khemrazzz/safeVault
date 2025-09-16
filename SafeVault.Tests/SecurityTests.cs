using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeVault.Core.Data;
using SafeVault.Core.Security;

namespace SafeVault.Tests;

[TestClass]
public class SecurityTests
{
    [TestMethod]
    public void InvalidEmail_ShouldFailValidation()
    {
        var validator = new InputValidator();
        Assert.IsFalse(validator.IsValidEmail("bad-email@@test"));
    }

    [TestMethod]
    public async Task SqlInjection_Attempt_ShouldNotReturnData()
    {
        await using var keepAliveConnection = new SqliteConnection("Data Source=:memory:;Mode=Memory;Cache=Shared");
        await keepAliveConnection.OpenAsync();

        using (var createCommand = keepAliveConnection.CreateCommand())
        {
            createCommand.CommandText = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                );";
            await createCommand.ExecuteNonQueryAsync();
        }

        using (var insertCommand = keepAliveConnection.CreateCommand())
        {
            insertCommand.CommandText = "INSERT INTO Users (Email, PasswordHash, Role) VALUES ($email, $passwordHash, $role);";
            insertCommand.Parameters.AddWithValue("$email", "user@example.com");
            insertCommand.Parameters.AddWithValue("$passwordHash", "hash");
            insertCommand.Parameters.AddWithValue("$role", "User");
            await insertCommand.ExecuteNonQueryAsync();
        }

        DbConnection ConnectionFactory()
        {
            var connection = new SqliteConnection("Data Source=:memory:;Mode=Memory;Cache=Shared");
            connection.Open();
            return connection;
        }

        var repository = new UserRepository(ConnectionFactory);

        var result = await repository.GetUserByEmailAsync("' OR 1=1 --");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void CommentSanitizer_ShouldEncodeHtml()
    {
        var sanitizer = new CommentSanitizer();
        var encoded = sanitizer.Encode("<script>alert('x')</script>");

        StringAssert.Contains(encoded, "&lt;script&gt;");
        StringAssert.Contains(encoded, "&lt;/script&gt;");
        Assert.IsFalse(encoded.Contains("<", StringComparison.Ordinal));
        Assert.IsFalse(encoded.Contains(">", StringComparison.Ordinal));
    }

    [TestMethod]
    public void WeakPassword_ShouldFailPolicy()
    {
        var policy = new PasswordPolicy();
        Assert.IsFalse(policy.IsStrongPassword("weakpass"));
    }
}

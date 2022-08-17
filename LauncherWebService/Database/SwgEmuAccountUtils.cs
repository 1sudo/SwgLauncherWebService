using System.Text;
using System.Security.Cryptography;

namespace LauncherWebService.Database;

internal static class SwgEmuAccountUtils
{
    /// <summary>
    /// Generates a 10 digit Station ID
    /// </summary>
    /// <returns>A 10 digit station ID</returns>
    private static int GenerateStationId() => new Random().Next(1000000000, 2147483647);

    /// <summary>
    /// Generates a random salt string
    /// </summary>
    /// <returns>A 32 character salt string</returns>
    private static string GenerateSalt()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Create().GetBytes(bytes);

        StringBuilder sb = new();

        // Convert bytes to UTF-16 string
        foreach (var b in bytes) sb.Append(b.ToString("x2"));

        // Shorten to 32 characters
        return sb.ToString()[..32];
    }

    /// <summary>
    /// Generates a hashed password
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns>A hashed password. Also returns generated salt if no salt provided.</returns>
    public static Tuple<string, string> HashPassword(string password, string? salt = null)
    {
        if (!string.IsNullOrEmpty(salt)) return Tuple.Create(ComputeHash(password, salt), "");
        var generatedSalt = GenerateSalt();
        return Tuple.Create(ComputeHash(password, generatedSalt), generatedSalt);
    }

    /// <summary>
    /// Hashes a password
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns>A password hash</returns>
    public static string ComputeHash(string password, string salt)
    {
        StringBuilder sb = new();

        var bytes = SHA256.Create().ComputeHash(
            Encoding.Default.GetBytes(Program.Settings!.DatabaseSecret + password + salt));

        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        
        return sb.ToString();
    }
}

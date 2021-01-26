namespace Rangle.Abstractions.Services
{
    public interface IDataProtectionService
    {
        string Encrypt(string text, string key);
        string Decrypt(string encryptedText, string key);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}

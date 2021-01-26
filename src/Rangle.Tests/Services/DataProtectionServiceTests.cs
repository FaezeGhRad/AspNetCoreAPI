using Rangle.Implementations.Services;
using Xunit;

namespace Rangle.Tests.Services
{
    public class DataProtectionServiceTests
    {
        [Theory]
        [InlineData("short-Text$%01@_", "skey-01!@#$_%^&*()|[]")]
        [InlineData("long-Textabcdefghijklmno_pqrstuvwxyz", "lkey-0123456789!@#$%^&*_()|[]?><~//\\")]
        public void EncryptDecrypt_Success(string text, string key)
        {
            // Arrange
            DataProtectionService service = new DataProtectionService();

            // Act
            string encrypted = service.Encrypt(text, key);
            string decrypted = service.Decrypt(encrypted, key);

            // Assert
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(decrypted);
            Assert.NotEqual(text, encrypted);
            Assert.Equal(text, decrypted);
        }

        [Theory]
        [InlineData("shortText$%01@", "skey01!@#$%^&*()|[]")]
        [InlineData("longTextabcdefghijklmnopqrstuvwxyz", "lkey0123456789!@#$%^&*()|[]?><~//\\")]
        public void EncryptDecrypt_Fail(string text, string key)
        {
            // Arrange
            DataProtectionService service = new DataProtectionService();

            // Act
            string encrypted = service.Encrypt(text, key);
            string decrypted = service.Decrypt(encrypted, key.Substring(1));

            // Assert
            Assert.NotEmpty(encrypted);
            Assert.Empty(decrypted);
            Assert.NotEqual(text, encrypted);
        }

    }
}

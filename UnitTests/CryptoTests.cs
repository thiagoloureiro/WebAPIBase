using Utils;
using Xunit;

namespace UnitTests
{
    public class CryptoTests
    {
        private const string strTest = "string_teste";

        [Fact]
        public void CryptDecrypt_Success()
        {
            // Act
            var encrypted = RijndaelManagedEncryption.EncryptRijndael(strTest);
            var decrypted = RijndaelManagedEncryption.DecryptRijndael(encrypted);

            // Assert
            if (decrypted == strTest)
                Assert.True(true);
        }
    }
}
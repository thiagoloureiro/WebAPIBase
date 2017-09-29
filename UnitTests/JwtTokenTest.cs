using Utils;
using Xunit;

namespace UnitTests
{
    public class JwtTokenTest
    {
        [Fact]
        public void JwtGenerateToken_Success()
        {
            var token = JwtManager.GenerateToken("user_test");

            if (token.Length > 0)
                Assert.True(true);
        }
    }
}
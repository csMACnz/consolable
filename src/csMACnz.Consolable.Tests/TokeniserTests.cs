using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class TokeniserTests
    {
        [Fact]
        public void Tokeniser_EmptyInput_EmptyOutput() 
        {
            var input = new string[0];
            var sut = new Tokeniser();

            var result = sut.GetTokens(input);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

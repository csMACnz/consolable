using System.Linq;
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

        [Fact]
        public void Tokeniser_SimpleSingleValueInput_OneResult() 
        {
            var input = new string[]{"simple"};
            var sut = new Tokeniser();

            var result = sut.GetTokens(input).ToList();

            Assert.NotNull(result);
            Assert.Collection(
                result,
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Value, t.TokenType);
                    Assert.Equal("simple", t.Value);
                });
        }
        
        [Fact]
        public void Tokeniser_SimpleSingleArgInput_OneResult() 
        {
            var input = new string[]{"-a"};
            var sut = new Tokeniser();

            var result = sut.GetTokens(input).ToList();

            Assert.NotNull(result);
            Assert.Collection(
                result,
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Arg, t.TokenType);
                    Assert.Equal("a", t.Value);
                });
        }
    }
}

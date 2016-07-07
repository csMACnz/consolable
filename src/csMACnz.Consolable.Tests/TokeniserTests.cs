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
        
        [Theory]
        [InlineDataAttribute("--", "-")]
        [InlineDataAttribute("-a", "a")]
        [InlineDataAttribute("/a", "a")]
        [InlineDataAttribute("--abort", "abort")]
        public void Tokeniser_SimpleSingleArgInput_OneResult(string rawArg, string parsedArg) 
        {
            var input = new string[]{rawArg};
            var sut = new Tokeniser();

            var result = sut.GetTokens(input).ToList();

            Assert.NotNull(result);
            Assert.Collection(
                result,
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Arg, t.TokenType);
                    Assert.Equal(parsedArg, t.Value);
                });
        }
        
        [Theory]
        [InlineDataAttribute("-a=value", "a", "value")]
        [InlineDataAttribute("-a:value", "a", "value")]
        [InlineDataAttribute("/a=value", "a", "value")]
        [InlineDataAttribute("/a:value", "a", "value")]
        [InlineDataAttribute("--abort=value", "abort", "value")]
        [InlineDataAttribute("--abort:value", "abort", "value")]
        [InlineDataAttribute("--abort:key=value", "abort", "key=value")]
        [InlineDataAttribute("--abort=key:value", "abort", "key:value")]
        [InlineDataAttribute("--abort=key=value", "abort", "key=value")]
        [InlineDataAttribute("--abort:key:value", "abort", "key:value")]
        public void Tokeniser_SimpleSingleArgInput_TwoResults(string rawArg, string parsedArg, string parsedValue) 
        {
            var input = new string[]{rawArg};
            var sut = new Tokeniser();

            var result = sut.GetTokens(input).ToList();

            Assert.NotNull(result);
            Assert.Collection(
                result,
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Arg, t.TokenType);
                    Assert.Equal(parsedArg, t.Value);
                },
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Value, t.TokenType);
                    Assert.Equal(parsedValue, t.Value);
                });
        }
    }
}

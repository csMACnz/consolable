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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("simple")]
        [InlineData("key:value")]
        [InlineData("key=value")]
        [InlineData("key value")]
        public void Tokeniser_SimpleSingleValueInput_OneResult(string value) 
        {
            var input = new string[]{value};
            var sut = new Tokeniser();

            var result = sut.GetTokens(input).ToList();

            Assert.NotNull(result);
            Assert.Collection(
                result,
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Value, t.TokenType);
                    Assert.Equal(value, t.Value);
                    Assert.Equal(value, t.Raw);
                    Assert.Equal(0, t.RawIndex);
                });
        }
        
        [Theory]
        [InlineDataAttribute("--", 1, "-")]
        [InlineDataAttribute("-a", 1, "a")]
        [InlineDataAttribute("/a", 1, "a")]
        [InlineDataAttribute("--abort", 2, "abort")]
        public void Tokeniser_SimpleSingleArgInput_OneResult(string rawArg, int rawIndex, string parsedArg) 
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
                    Assert.Equal(rawArg, t.Raw);
                    Assert.Equal(rawIndex, t.RawIndex);
                });
        }
        
        [Theory]
        [InlineDataAttribute("-a=value", "a", "value")]
        [InlineDataAttribute("-a=key=value", "a", "key=value")]
        [InlineDataAttribute("-a=key:value", "a", "key:value")]
        [InlineDataAttribute("-a:value", "a", "value")]
        [InlineDataAttribute("-a:key=value", "a", "key=value")]
        [InlineDataAttribute("-a:key:value", "a", "key:value")]
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


        [Theory]
        [InlineDataAttribute("-ab=value", "a", "b", "value")]
        public void Tokeniser_SimpleSingleArgInput_TwoResults(string rawArg, string parsedArg1, string parsedArg2, string parsedValue) 
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
                    Assert.Equal(parsedArg1, t.Value);
                },
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Arg, t.TokenType);
                    Assert.Equal(parsedArg2, t.Value);
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

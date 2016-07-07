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
        [InlineDataAttribute("-a=value", 1, "a", 3, "value")]
        [InlineDataAttribute("-a=key=value", 1, "a", 3, "key=value")]
        [InlineDataAttribute("-a=key:value", 1, "a", 3, "key:value")]
        [InlineDataAttribute("-a:value", 1, "a", 3, "value")]
        [InlineDataAttribute("-a:key=value", 1, "a", 3, "key=value")]
        [InlineDataAttribute("-a:key:value", 1, "a", 3, "key:value")]
        [InlineDataAttribute("/a=value", 1, "a", 3, "value")]
        [InlineDataAttribute("/a:value", 1, "a", 3, "value")]
        [InlineDataAttribute("--abort=value", 2, "abort", 8, "value")]
        [InlineDataAttribute("--abort:value", 2, "abort", 8, "value")]
        [InlineDataAttribute("--abort:key=value", 2, "abort", 8, "key=value")]
        [InlineDataAttribute("--abort=key:value", 2, "abort", 8, "key:value")]
        [InlineDataAttribute("--abort=key=value", 2, "abort", 8, "key=value")]
        [InlineDataAttribute("--abort:key:value", 2, "abort", 8, "key:value")]
        public void Tokeniser_SimpleSingleArgInput_TwoResults(string rawArg, int argIndex, string parsedArg, int valueIndex, string parsedValue) 
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
                    Assert.Equal(argIndex, t.RawIndex);
                },
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Value, t.TokenType);
                    Assert.Equal(parsedValue, t.Value);
                    Assert.Equal(rawArg, t.Raw);
                    Assert.Equal(valueIndex, t.RawIndex);
                });
        }


        [Theory]
        [InlineDataAttribute("-ab=value", 1, "a", 2, "b", 4, "value")]
        public void Tokeniser_SimpleSingleArgInput_TwoResults(string rawArg, int arg1Index, string parsedArg1, int arg2Index, string parsedArg2, int valueIndex, string parsedValue) 
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
                    Assert.Equal(rawArg, t.Raw);
                    Assert.Equal(arg1Index, t.RawIndex);
                },
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Arg, t.TokenType);
                    Assert.Equal(parsedArg2, t.Value);
                    Assert.Equal(rawArg, t.Raw);
                    Assert.Equal(arg2Index, t.RawIndex);
                },
                t=> 
                {
                    Assert.NotNull(t);
                    Assert.Equal(TokenType.Value, t.TokenType);
                    Assert.Equal(parsedValue, t.Value);
                    Assert.Equal(rawArg, t.Raw);
                    Assert.Equal(valueIndex, t.RawIndex);
                });
        }
    }
}

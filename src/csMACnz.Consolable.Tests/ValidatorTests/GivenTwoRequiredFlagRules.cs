using Xunit;

namespace csMACnz.Consolable.Tests.ValidatorTests
{
    public class GivenTwoRequiredFlagRules
    {
        private IRule[] _rules;
        public GivenTwoRequiredFlagRules()
        {
            _rules = new IRule[] {
                new RequiredArgument('a', "alpha"),
                new RequiredArgument('b', "bravo")
            };
        }

        [Fact]
        public void ValidateArguments_UnexpectedValueTokenThenValidArgument_OneError()
        {
            var input = new[] {
                new Token(TokenType.Arg, "a", "-a", 1),
                new Token(TokenType.Value, "hello", "hello", 0),
                new Token(TokenType.Arg, "b", "-b", 1)
            };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnexpectedArgValue, e.Type);
                    Assert.Equal("hello", e.ErrorToken.Value);
                    Assert.Equal(0, e.ErrorToken.RawIndex);
                    Assert.Equal("hello", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

    }
}
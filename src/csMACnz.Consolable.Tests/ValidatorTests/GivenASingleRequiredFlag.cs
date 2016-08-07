using Xunit;

namespace csMACnz.Consolable.Tests.ValidatorTests
{
    public class GivenASingleRequiredFlag
    {
        private IRule[] _rules;
        public GivenASingleRequiredFlag()
        {
            _rules = new IRule[] { new RequiredArgument('a', "alpha") };
        }

        [Fact]
        public void ValidateArguments_ValidToken_NoErrors()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1) };
            
            var result = Validator.ValidateArguments(_rules, input);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidateArguments_MissingRequiredToken_OneError()
        {
            var input = new Token[0];

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.RequiredArgMissing, e.Type);
                    Assert.Null(e.ErrorToken);
                    Assert.Equal("alpha", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_DuplicateTokens_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Arg, "a", "-a", 1) };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.DuplicateArg, e.Type);
                    Assert.Equal("a", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-a", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_UnexpectedValueToken_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Value, "hello", "hello", 0) };

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
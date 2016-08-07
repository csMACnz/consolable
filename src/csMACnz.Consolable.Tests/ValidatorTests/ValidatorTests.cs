using Xunit;

namespace csMACnz.Consolable.Tests.ValidatorTests
{
    public class ValidatorTests
    {

        [Fact]
        public void ValidateArguments_ValidMultiValueRuleSetWithMissingSingleValueToken_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1) };
            var rules = new[] { new RequiredArgument('a', "alpha", ArgumentMode.MultiValue) };

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.MissingValue, e.Type);
                    Assert.Equal("a", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-a", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithMissingSingleValueTokenThenValidArgument_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Arg, "b", "-b", 1) };
            var rules = new[] { new RequiredArgument('a', "alpha", ArgumentMode.SingleValue), new RequiredArgument('b', "bravo", ArgumentMode.Flag) };

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.MissingValue, e.Type);
                    Assert.Equal("a", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-a", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithMissingMultiValueTokenThenValidArgument_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Arg, "b", "-b", 1) };
            var rules = new[] { new RequiredArgument('a', "alpha", ArgumentMode.MultiValue), new RequiredArgument('b', "bravo", ArgumentMode.Flag) };

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.MissingValue, e.Type);
                    Assert.Equal("a", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-a", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }
    }
}
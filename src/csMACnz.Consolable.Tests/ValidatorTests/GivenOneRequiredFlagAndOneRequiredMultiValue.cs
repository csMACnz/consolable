using Xunit;

namespace csMACnz.Consolable.Tests.ValidatorTests
{
    public class GivenOneRequiredFlagAndOneRequiredMultiValue
    {
        private readonly IRule[] _rules;

        public GivenOneRequiredFlagAndOneRequiredMultiValue()
        {
            _rules = new IRule[]
            {
                new RequiredArgument('a', "alpha", ArgumentMode.Flag),
                new RequiredArgument('b', "bravo", ArgumentMode.MultiValue)
            };
        }

        [Fact]
        public void ValidateArguments_MissingMultiValueTokenThenValidArgument_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "b", "-b", 1), new Token(TokenType.Arg, "a", "-a", 1) };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.MissingValue, e.ErrorType);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                });
        }

        [Fact]
        public void ValidateArguments_ValidArgumentThenMissingMultiValueToken_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Arg, "b", "-b", 1) };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.MissingValue, e.ErrorType);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                });
        }
    }
}

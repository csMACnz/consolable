using Xunit;

namespace csMACnz.Consolable.Tests.ValidatorTests
{
    public class GivenASingleRequiredMultipleValueArg
    {
        private IRule[] _rules;
        public GivenASingleRequiredMultipleValueArg()
        {
            _rules = new IRule[] { new RequiredArgument('a', "alpha", ArgumentMode.MultiValue) };
        }
        
        [Fact]
        public void ValidateArguments_ExpectedValueToken_NoErrors()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1), new Token(TokenType.Value, "hello", "hello", 0) };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidateArguments_MissingValueToken_OneError()
        {
            var input = new[] { new Token(TokenType.Arg, "a", "-a", 1) };
            
            var result = Validator.ValidateArguments(_rules, input);

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

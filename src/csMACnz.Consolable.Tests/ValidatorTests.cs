using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class  ValidatorTests
    {
        [Fact]
        public void ValidateArguments_ValidRuleSetWithValidTokens_NoErrors()
        {
            var input = new []{new Token(TokenType.Arg, "a", "-a",1){}};
            var rules = new []{new RequiredArgument('a', "alpha")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithUnknownToken_OneUnknownArgumentError()
        {
            var input = new []{new Token(TokenType.Arg, "b", "-b",1){}};
            var rules = new []{new RequiredArgument('a', "alpha")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
                    Assert.NotNull(e);
                }
            );
        }
    }
}
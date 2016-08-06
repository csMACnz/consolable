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
        public void ValidateArguments_ValidRuleSetWithExpectedValueToken_NoErrors()
        {
            var input = new []{new Token(TokenType.Arg, "a", "-a",1){}, new Token(TokenType.Value, "hello", "hello", 0)};
            var rules = new []{new RequiredArgument('a', "alpha", ArgumentMode.SingleValue)};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithUnexpectedValueToken_OneError()
        {
            var input = new []{new Token(TokenType.Arg, "a", "-a",1){}, new Token(TokenType.Value, "hello", "hello", 0)};
            var rules = new []{new RequiredArgument('a', "alpha")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnexpectedValue, e.Type);
                    Assert.Equal("hello", e.ErrorToken.Value);
                    Assert.Equal(0, e.ErrorToken.RawIndex);
                    Assert.Equal("hello", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithUnexpectedValueTokenThenValidArgument_OneError()
        {
            var input = new []{new Token(TokenType.Arg, "a", "-a",1), new Token(TokenType.Value, "hello", "hello", 0), new Token(TokenType.Arg, "b", "-b", 1)};
            var rules = new []{new RequiredArgument('a', "alpha"),new RequiredArgument('b', "bravo")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnexpectedValue, e.Type);
                    Assert.Equal("hello", e.ErrorToken.Value);
                    Assert.Equal(0, e.ErrorToken.RawIndex);
                    Assert.Equal("hello", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_ValidRuleSetWithMissingSingleValueToken_OneError()
        {
            var input = new []{new Token(TokenType.Arg, "a", "-a",1){}};
            var rules = new []{new RequiredArgument('a', "alpha", ArgumentMode.SingleValue)};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
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
            var input = new []{new Token(TokenType.Arg, "a", "-a",1),new Token(TokenType.Arg, "b", "-b",1)};
            var rules = new []{new RequiredArgument('a', "alpha", ArgumentMode.SingleValue),new RequiredArgument('b', "bravo", ArgumentMode.NoValue)};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
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
        public void ValidateArguments_ValidRuleSetWithUnknownToken_OneUnknownArgumentError()
        {
            var input = new []{new Token(TokenType.Arg, "b", "-b",1){}};
            var rules = new []{new RequiredArgument('a', "alpha")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.Type);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                }
            );
        }
        
        [Fact]
        public void ValidateArguments_ValidRuleSetWithTwoUnknownTokens_TwoUnknownArgumentError()
        {
            var input = new []{
                new Token(TokenType.Arg, "b", "-b",1){},
                new Token(TokenType.Arg, "c", "-c",1){}
            };
            var rules = new []{new RequiredArgument('a', "alpha")};

            var result = Validator.ValidateArguments(rules, input);

            Assert.Collection(
                result,
                e =>{
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.Type);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                },
                e =>{
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.Type);
                    Assert.Equal("c", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-c", e.ErrorToken.Raw);
                    Assert.Equal("c", e.Argument);
                }
            );
        }
    }
}
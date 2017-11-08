using System;
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
            var input = Array.Empty<Token>();

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.RequiredArgMissing, e.ErrorType);
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
                    Assert.Equal(ErrorType.DuplicateArg, e.ErrorType);
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
                    Assert.Equal(ErrorType.UnexpectedArgValue, e.ErrorType);
                    Assert.Equal("hello", e.ErrorToken.Value);
                    Assert.Equal(0, e.ErrorToken.RawIndex);
                    Assert.Equal("hello", e.ErrorToken.Raw);
                    Assert.Equal("a", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_UnknownArgToken_OneUnknownArgumentError()
        {
            var input = new[]{
                new Token(TokenType.Arg, "a", "-a",1),
                new Token(TokenType.Arg, "b", "-b",1)};

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.ErrorType);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_UnknownInitialValueToken_OneUnexpectedStartPositionalValueError()
        {
            var input = new[] {
                new Token(TokenType.Value, "blue", "blue", 0),
                new Token(TokenType.Arg, "a", "-a", 1)
            };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnexpectedStartPositionalValue, e.ErrorType);
                    Assert.Equal("blue", e.ErrorToken.Value);
                    Assert.Equal(0, e.ErrorToken.RawIndex);
                    Assert.Equal("blue", e.ErrorToken.Raw);
                    Assert.Null(e.Argument);
                }
            );
        }

        [Fact]
        public void ValidateArguments_TwoUnknownTokens_TwoUnknownArgumentErrors()
        {
            var input = new[]{
                new Token(TokenType.Arg, "a", "-a",1),
                new Token(TokenType.Arg, "b", "-b",1),
                new Token(TokenType.Arg, "c", "-c",1)
            };

            var result = Validator.ValidateArguments(_rules, input);

            Assert.Collection(
                result,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.ErrorType);
                    Assert.Equal("b", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-b", e.ErrorToken.Raw);
                    Assert.Equal("b", e.Argument);
                },
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal(ErrorType.UnknownArgument, e.ErrorType);
                    Assert.Equal("c", e.ErrorToken.Value);
                    Assert.Equal(1, e.ErrorToken.RawIndex);
                    Assert.Equal("-c", e.ErrorToken.Raw);
                    Assert.Equal("c", e.Argument);
                }
            );
        }
    }
}

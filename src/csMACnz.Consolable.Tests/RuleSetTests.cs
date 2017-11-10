using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class RuleSetTests
    {
        [Fact]
        public void GetValidArguments_EmptyRules_ReturnsNoResults()
        {
            var input = Enumerable.Empty<IRule>();

            var results = RuleSet.GetValidArguments(input);

            Assert.Empty(results);
        }

        [Fact]
        public void GetValidArguments_SingleRequiredRule_ReturnsOneResult()
        {
            var input = new IRule[] { new RequiredArgument('a', "alpha") };

            var results = RuleSet.GetValidArguments(input);

            Assert.Collection(
                results,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('a', e.ShortName);
                    Assert.Equal("alpha", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                });
        }

        [Fact]
        public void GetValidArguments_TwoRequiredRules_ReturnsTwoResults()
        {
            var input = new IRule[] { new RequiredArgument('a', "alpha"), new RequiredArgument('b', "bravo") };

            var results = RuleSet.GetValidArguments(input);

            Assert.Collection(
                results,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('a', e.ShortName);
                    Assert.Equal("alpha", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                },
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('b', e.ShortName);
                    Assert.Equal("bravo", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                });
        }

        [Fact]
        public void GetValidArguments_SingleArgumentTestRule_ReturnsOneResult()
        {
            var input = new IRule[] { new SingleArgumentTestRule() };

            var results = RuleSet.GetValidArguments(input);

            Assert.Collection(
                results,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('a', e.ShortName);
                    Assert.Equal("alpha", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                });
        }

        [Fact]
        public void GetValidArguments_NoArgumentsTestRule_ReturnsOneResult()
        {
            var input = new IRule[] { new NoArgumentsTestRule() };

            var results = RuleSet.GetValidArguments(input);

            Assert.Empty(results);
        }

        [Fact]
        public void GetValidArguments_MultipleArgumentsTestRule_ReturnsThreeResults()
        {
            var input = new IRule[] { new MultipleArgumentsTestRule() };

            var results = RuleSet.GetValidArguments(input);

            Assert.Collection(
                results,
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('a', e.ShortName);
                    Assert.Equal("alpha", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                },
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('b', e.ShortName);
                    Assert.Equal("bravo", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                },
                e =>
                {
                    Assert.NotNull(e);
                    Assert.Equal('c', e.ShortName);
                    Assert.Equal("charlie", e.LongName);
                    Assert.Equal(ArgumentMode.Flag, e.ValueMode);
                });
        }

        private class NoArgumentsTestRule : IRule
        {
            public IEnumerable<Argument> GetArguments()
            {
                return Enumerable.Empty<Argument>();
            }

            public IEnumerable<ParseError> ValidateArguments(List<Argument> providedArguments)
            {
                return Enumerable.Empty<ParseError>();
            }

            public VerifyMode Verify(List<Argument> providedArguments)
            {
                throw new NotImplementedException();
            }
        }

        private class SingleArgumentTestRule : IRule
        {
            public IEnumerable<Argument> GetArguments()
            {
                return new[] { new Argument { ShortName = 'a', LongName = "alpha", ValueMode = ArgumentMode.Flag } };
            }

            public IEnumerable<ParseError> ValidateArguments(List<Argument> providedArguments)
            {
                return Enumerable.Empty<ParseError>();
            }

            public VerifyMode Verify(List<Argument> providedArguments)
            {
                throw new NotImplementedException();
            }
        }

        private class MultipleArgumentsTestRule : IRule
        {
            public IEnumerable<Argument> GetArguments()
            {
                return new[]
                {
                    new Argument { ShortName = 'a', LongName = "alpha", ValueMode = ArgumentMode.Flag },
                    new Argument { ShortName = 'b', LongName = "bravo", ValueMode = ArgumentMode.Flag },
                    new Argument { ShortName = 'c', LongName = "charlie", ValueMode = ArgumentMode.Flag }
                };
            }

            public IEnumerable<ParseError> ValidateArguments(List<Argument> providedArguments)
            {
                return Enumerable.Empty<ParseError>();
            }

            public VerifyMode Verify(List<Argument> providedArguments)
            {
                throw new NotImplementedException();
            }
        }
    }
}
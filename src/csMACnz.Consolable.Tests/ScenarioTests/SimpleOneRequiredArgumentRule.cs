using Xunit;

namespace csMACnz.Consolable.Tests.ScenarioTests
{
    public class SimpleOneRequiredArgumentRule
    {
        private readonly IRule[] _rules;

        public SimpleOneRequiredArgumentRule()
        {
            _rules = new IRule[] { new RequiredArgument('a', "alpha") };
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("/a")]
        [InlineData("--alpha")]
        public void ValidArgumentPasses(string arg)
        {
            // Simulate .Net args parsing
            var args = ShellArgsParser.Parse(arg);

            var values = CommandLineParser.Parse(_rules, args, error => Assert.False(true, "Errors Not Expected."));

            var boolValue = Assert.IsType<bool>(values["alpha"]);
            Assert.True(boolValue);
        }

        [Fact]
        public void EmptyArguments_ErrorResult()
        {
            // Simulate .Net args parsing
            var args = ShellArgsParser.Parse(string.Empty);

            bool errorWasCalled = false;
            var values = CommandLineParser.Parse(
                _rules,
                args,
                errors =>
                {
                    errorWasCalled = true;
                    Assert.Collection(
                        errors,
                        e => Assert.Equal(ErrorType.RequiredArgMissing, e.ErrorType));
                });

            Assert.True(errorWasCalled);
            Assert.NotNull(values);
            Assert.Empty(values);
        }

        [Theory]
        [InlineData("-b")]
        [InlineData("/b")]
        [InlineData("--bravo")]
        public void IncorrectArgument_ErrorResult(string arg)
        {
            // Simulate .Net args parsing
            var args = ShellArgsParser.Parse(arg);

            bool errorWasCalled = false;
            var values = CommandLineParser.Parse(
                _rules,
                args,
                errors =>
                {
                    errorWasCalled = true;
                    Assert.Collection(
                            errors,
                            e => Assert.Equal(ErrorType.UnknownArgument, e.ErrorType),
                            e => Assert.Equal(ErrorType.RequiredArgMissing, e.ErrorType));
                });

            Assert.True(errorWasCalled);
            Assert.NotNull(values);
            Assert.Empty(values);
        }
    }
}

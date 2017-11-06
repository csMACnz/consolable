using Xunit;

namespace csMACnz.Consolable.Tests.ScenarioTests
{
    public class SimpleOneRequiredRule
    {
        private IRule[] _rules;
        public SimpleOneRequiredRule()
        {
            _rules = new IRule[] { new RequiredArgument('a', "alpha") };
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("/a")]
        [InlineData("--alpha")]
        public void ValidArgumentPasses(string arg)
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse(arg);

            var values = Consolable.Parse(_rules, args, error => Assert.False(true, "Errors Not Expected."));

            var boolValue = Assert.IsType<bool>(values["alpha"]);
            Assert.True(boolValue);
        }

        [Fact]
        public void EmptyArguments_ErrorResult()
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse("");

            bool errorWasCalled = false;
            var values = Consolable.Parse(
                _rules,
                args,
                errors =>
                {
                    errorWasCalled = true;
                    Assert.Collection(
                        errors,
                        e => Assert.Equal(ErrorType.RequiredArgMissing, e.Type));
                });

            Assert.True(errorWasCalled);
        }

        [Theory]
        [InlineData("-b")]
        [InlineData("/b")]
        [InlineData("--bravo")]
        public void IncorrectArgument_ErrorResult(string arg)
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse(arg);

            bool errorWasCalled = false;
            var values = Consolable.Parse(
                _rules,
                args,
                errors =>
                {
                    errorWasCalled = true;
                    Assert.Collection(
                            errors,
                            e => Assert.Equal(ErrorType.UnknownArgument, e.Type),
                            e => Assert.Equal(ErrorType.RequiredArgMissing, e.Type));
                });

            Assert.True(errorWasCalled);
        }
    }
}
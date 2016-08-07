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
        [InlineData("--alpha")]
        public void ValidArgumentPasses(string arg)
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse(arg);

            var values = Consolable.Parse(_rules, args, error=> Assert.False(true, "Errors Not Expected."));
            
            Assert.Equal(true, values["alpha"]);
        }

    }
}
using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class UsageTests
    {
        [Fact]
        public void BareBonesHelpTest()
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse("-?");
            var rules = new IRule[] { new RequiredArgument('?', "help") };
            
            var tokens = new Tokeniser().GetTokens(args);
            var errors = Validator.ValidateArguments(rules, tokens);
            Assert.Empty(errors);
            //TODO: provide values dictionary results from tokens and rules
            //TODO: encapsulate into helper class
        }

        [Fact]
        public void BareBonesUsageTest()
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse("-a -b Value -c Item1 Item2 -de --flag --green true");
            var rules = new IRule[] {
                 new RequiredArgument('a', "alpha"),
                 new RequiredArgument('b', "bravo", ArgumentMode.SingleValue),
                 new RequiredArgument('c', "charlie", ArgumentMode.MultiValue),
                 new RequiredArgument('d', "delta"),
                 new RequiredArgument('e', "echo"),
                 new RequiredArgument('f', "flag"),
                 new RequiredArgument('g', "green", ArgumentMode.SingleValue),//Investigate if this should be flag feature (allow boolean value)
            };
            var tokens = new Tokeniser().GetTokens(args);
            var errors = Validator.ValidateArguments(rules, tokens);
            Assert.Empty(errors);
            //TODO: provide values dictionary results from tokens and rules
            //TODO: encapsulate into helper class
        }
    }
}
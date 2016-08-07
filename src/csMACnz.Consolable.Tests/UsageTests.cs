using System.Collections.Generic;
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

            var values = Consolable.Parse(rules, args, error=> Assert.False(true, "Errors Not Expected."));
            
            Assert.Equal(true, values["help"]);
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
            
            var values = Consolable.Parse(rules, args, error=> Assert.False(true, "Errors Not Expected."));
            
            Assert.Equal(true, values["alpha"]);
            Assert.Equal("Value", values["bravo"]);
            Assert.Collection((List<string>)values["charlie"],
                v => Assert.Equal("Item1", v),
                v => Assert.Equal("Item2", v)
            );
            Assert.Equal(true, values["delta"]);
            Assert.Equal(true, values["echo"]);
            Assert.Equal(true, values["flag"]);
            Assert.Equal("true", values["green"]);
        }
    }
}
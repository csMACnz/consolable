using System;
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

            var values = CommandLineParser.Parse(rules, args, error=> Assert.False(true, "Errors Not Expected."));
            
            AssertContainsKeyWithBoolValueTrue(values, "help");
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
            
            var values = CommandLineParser.Parse(rules, args, error=> Assert.False(true, "Errors Not Expected."));

            AssertContainsKeyWithBoolValueTrue(values, "alpha");
            Assert.Equal("Value", values["bravo"]);
            Assert.Collection((List<string>)values["charlie"],
                v => Assert.Equal("Item1", v),
                v => Assert.Equal("Item2", v)
            );
            AssertContainsKeyWithBoolValueTrue(values, "delta");
            AssertContainsKeyWithBoolValueTrue(values, "echo");
            AssertContainsKeyWithBoolValueTrue(values, "flag");
            Assert.Equal("true", values["green"]);
        }

        private void AssertContainsKeyWithBoolValueTrue(Dictionary<string, object> values, string key)
        {
            Assert.True(values.ContainsKey(key));
            var boolValue = Assert.IsType<bool>(values[key]);
            Assert.True(boolValue);
        }
    }
}

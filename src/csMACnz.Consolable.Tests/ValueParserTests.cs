using System.Collections.Generic;
using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class ValueParserTests
    {
        [Fact]
        public void SimpleOneFlagArgTest()
        {
            var tokens = new[] { new Token(TokenType.Arg, "a", "-a", 1) { } };
            var rules = new[] { new RequiredArgument('a', "alpha") };

            Dictionary<string, object> values = new ValueParser(rules).Parse(tokens);
            Assert.NotNull(values);
            Assert.NotEmpty(values);
            Assert.True(values.ContainsKey("alpha"));
            
            var boolValue = Assert.IsType<bool>(values["alpha"]);
            Assert.True(boolValue);
        }
    }
}
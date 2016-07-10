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
    }
}
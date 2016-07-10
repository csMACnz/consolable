using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class RuleSet
    {
        public static IEnumerable<Argument> GetValidArguments(IEnumerable<IRule> input)
        {
            return input.SelectMany(i => i.GetArguments());
        }
    }
}
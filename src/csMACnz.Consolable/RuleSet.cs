using System;
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

    public interface IRule
    {
        IEnumerable<Argument> GetArguments();
    }

    public class RequiredArgument : IRule
    {
        public string LongName { get; }
        public char ShortName { get; }
        public ValueMode ValueMode { get; }
        public RequiredArgument(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
            ValueMode = ValueMode.NoValue;
        }

        public IEnumerable<Argument> GetArguments()
        {
            return new[] {
                 new Argument{
                     ShortName = ShortName,
                     LongName = LongName,
                     ValueMode = ValueMode,
                 }
            };
        }
    }

    public class Argument
    {
        public string LongName { get; set; }
        public char ShortName { get; set; }
        public ValueMode ValueMode { get; set; }
    }

    public enum ValueMode
    {
        Invalid,
        NoValue,
        SingleValue,
        MultiValue
    }
}
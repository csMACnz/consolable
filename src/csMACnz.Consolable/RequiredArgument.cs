using System.Collections.Generic;

namespace csMACnz.Consolable
{
    
    public class RequiredArgument : IRule
    {
        public string LongName { get; }
        public char ShortName { get; }
        public ArgumentMode ValueMode { get; }
        
        public RequiredArgument(char shortName, string longName)
        : this(shortName, longName, ArgumentMode.NoValue)
        {
        }

        public RequiredArgument(char shortName, string longName, ArgumentMode argumentMode)
        {
            ShortName = shortName;
            LongName = longName;
            ValueMode = argumentMode;
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
}
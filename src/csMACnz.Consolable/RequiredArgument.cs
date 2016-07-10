using System.Collections.Generic;

namespace csMACnz.Consolable
{
    
    public class RequiredArgument : IRule
    {
        public string LongName { get; }
        public char ShortName { get; }
        public ArgumentMode ValueMode { get; }
        public RequiredArgument(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
            ValueMode = ArgumentMode.NoValue;
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
using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public class RequiredArgument : IRule
    {
        public string LongName { get; }

        public char ShortName { get; }

        public ArgumentMode ValueMode { get; }

        public RequiredArgument(char shortName, string longName)
        : this(shortName, longName, ArgumentMode.Flag)
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
            return new[]
            {
                 new Argument
                {
                     ShortName = ShortName,
                     LongName = LongName,
                     ValueMode = ValueMode,
                 }
            };
        }

        public IEnumerable<ParseError> ValidateArguments(List<Argument> providedArguments)
        {
            if (!providedArguments.Any(a => LongName == a.LongName))
            {
                yield return new ParseError
                {
                    ErrorType = ErrorType.RequiredArgMissing,
                    ErrorToken = null,
                    Argument = LongName
                };
            }
        }

        public VerifyMode Verify(List<Argument> providedArguments)
        {
            if (providedArguments.Any(a => LongName == a.LongName))
            {
                return VerifyMode.ExplicitlyProvided;
            }

            return VerifyMode.NotProvided;
        }
    }
}

using System.Collections.Generic;

namespace csMACnz.Consolable
{
    public interface IRule
    {
        IEnumerable<Argument> GetArguments();
        IEnumerable<Error> ValidateArguments(List<Argument> providedArguments);
    }
}
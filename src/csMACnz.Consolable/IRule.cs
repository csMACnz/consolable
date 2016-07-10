using System.Collections.Generic;

namespace csMACnz.Consolable
{
    public interface IRule
    {
        IEnumerable<Argument> GetArguments();
    }
}
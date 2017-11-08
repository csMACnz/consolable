using System.Collections.Generic;

namespace csMACnz.Consolable
{
    public interface IRule
    {
        IEnumerable<Argument> GetArguments();
        VerifyMode Verify(List<Argument> providedArguments);
        IEnumerable<ParseError> ValidateArguments(List<Argument> providedArguments);
    }

    public enum VerifyMode
    {
        ExplicitlyProvided,
        ImplicitlyProvided,
        NotProvided
    }
}
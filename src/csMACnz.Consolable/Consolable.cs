using System;
using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class Consolable
    {
        public static Dictionary<string, object> Parse(IRule[] rules, string[] args, Action<IEnumerable<Error>> onError)
        {
            var tokens = new Tokeniser().GetTokens(args);
            var errors = Validator.ValidateArguments(rules, tokens);
            if (errors != null && errors.Any())
            {
                onError(errors);
                return new Dictionary<string, object>();
            }
            return  new ValueParser(rules).Parse(tokens.ToArray());
        }
    }
}

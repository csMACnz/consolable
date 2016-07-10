using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class Validator
    {
        public static IEnumerable<Error> ValidateArguments(IEnumerable<IRule> rules, IEnumerable<Token> tokens)
        {
            var arguments = RuleSet.GetValidArguments(rules);

            foreach(var token in tokens)
            {
                if(token.TokenType == TokenType.Arg){
                    if(!arguments.Any(a=>a.LongName == token.Value || a.ShortName+"" == token.Value))
                    {
                        yield return new Error(); 
                    }
                }
            }
        }
    }

    public class Error
    {
        
    }
}
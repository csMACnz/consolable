using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class Validator
    {
        public static IEnumerable<Error> ValidateArguments(IEnumerable<IRule> rules, IEnumerable<Token> tokens)
        {
            var arguments = RuleSet.GetValidArguments(rules);

            Token lastToken = null;
            Token lastArgToken = null;
            Argument lastArg = null;
            Token lastArgValueToken = null;
            foreach(var token in tokens)
            {
                lastToken = token;
                if(token.TokenType == TokenType.Arg){
                    if(lastArg != null)
                    {
                        if(lastArg.ValueMode == ArgumentMode.SingleValue)
                        {
                            yield return new Error
                            {
                                Type = ErrorType.MissingValue,
                                ErrorToken = lastArgToken 
                            };
                        }
                    }
                    lastArgToken = token;
                    lastArgValueToken = null;
                    lastArg = arguments.SingleOrDefault(a=>a.LongName == token.Value || a.ShortName+"" == token.Value); 
                    if(lastArg == null)
                    {
                        yield return new Error
                        {
                            Type = ErrorType.UnknownArgument,
                            ErrorToken = token 
                        };
                    }
                }
                if(token.TokenType == TokenType.Value){
                    if(lastArgToken != null)
                    {
                        if(lastArg != null)
                        {
                            if(lastArgValueToken == null)
                            {
                                if(lastArg.ValueMode == ArgumentMode.NoValue)
                                {
                                    yield return new Error
                                    {
                                        Type = ErrorType.UnexpectedValue,
                                        ErrorToken = token 
                                    };
                                }
                            }
                        }
                    }
                }
            }
            if(lastToken != null && lastToken.TokenType == TokenType.Arg)
            {
                if(lastArg != null)
                {
                    if(lastArg.ValueMode == ArgumentMode.SingleValue)
                    {
                        yield return new Error
                        {
                            Type = ErrorType.MissingValue,
                            ErrorToken = lastArgToken 
                        };
                    }
                }
            }
        }
    }

    public class Error
    {
        public ErrorType Type { get; set; }
        public Token ErrorToken { get; set; }
    }

    public enum ErrorType
    {
        UnknownArgument,
        UnexpectedValue,
        MissingValue
    }
}
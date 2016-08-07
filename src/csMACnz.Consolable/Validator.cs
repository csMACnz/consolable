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
            var providedTokens = new List<Token>();
            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Arg)
                {
                    providedTokens.Add(token);
                    if (lastArg != null)
                    {
                        if (lastToken == lastArgToken && (lastArg.ValueMode == ArgumentMode.SingleValue || lastArg.ValueMode == ArgumentMode.MultiValue))
                        {
                            yield return new Error
                            {
                                Type = ErrorType.MissingValue,
                                ErrorToken = lastArgToken,
                                Argument = lastArgToken.Value
                            };
                        }
                    }
                    lastArgToken = token;
                    lastArgValueToken = null;
                    lastArg = arguments.SingleOrDefault(a => ArgMatchesToken(a, token));
                    if (lastArg == null)
                    {
                        yield return new Error
                        {
                            Type = ErrorType.UnknownArgument,
                            ErrorToken = token,
                            Argument = token.Value
                        };
                    }
                }
                else if (token.TokenType == TokenType.Value)
                {
                    if (lastArgToken != null)
                    {
                        if (lastArg != null)
                        {
                            if (lastArgValueToken == null)
                            {
                                if (lastArg.ValueMode == ArgumentMode.Flag)
                                {
                                    yield return new Error
                                    {
                                        Type = ErrorType.UnexpectedArgValue,
                                        ErrorToken = token,
                                        Argument = lastArgToken.Value
                                    };
                                }
                            }
                        }
                    }
                    else
                    {
                        yield return new Error
                        {
                            Type = ErrorType.UnexpectedStartPositionalValue,
                            ErrorToken = token,
                            Argument = null
                        };
                    }
                }
                lastToken = token;
            }
            if (lastToken != null && lastToken.TokenType == TokenType.Arg)
            {
                if (lastArg != null)
                {
                    if (lastArg.ValueMode == ArgumentMode.SingleValue || lastArg.ValueMode == ArgumentMode.MultiValue)
                    {
                        yield return new Error
                        {
                            Type = ErrorType.MissingValue,
                            ErrorToken = lastArgToken,
                            Argument = lastArgToken.Value
                        };
                    }
                }
            }

            foreach (var argument in arguments)
            {
                if (!tokens.Any(t => ArgMatchesToken(argument, t)))
                {
                    yield return new Error
                    {
                        Type = ErrorType.RequiredArgMissing,
                        ErrorToken = null,
                        Argument = argument.LongName
                    };
                }
            }
        }

        private static bool ArgMatchesToken(Argument argument, Token token)
        {
            return argument.LongName == token.Value || argument.ShortName + "" == token.Value;
        }
    }

    public class Error
    {
        public ErrorType Type { get; set; }
        public Token ErrorToken { get; set; }
        public string Argument { get; set; }
    }

    public enum ErrorType
    {
        UnknownArgument,
        UnexpectedArgValue,
        UnexpectedStartPositionalValue,
        MissingValue,
        RequiredArgMissing
    }
}
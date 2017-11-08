using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class Validator
    {
        public static IEnumerable<ParseError> ValidateArguments(IEnumerable<IRule> rules, IEnumerable<Token> tokens)
        {
            var arguments = RuleSet.GetValidArguments(rules);

            Token lastToken = null;
            Token lastArgToken = null;
            Argument lastArg = null;
            Token lastArgValueToken = null;
            var providedArguments = new List<Argument>();
            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Arg)
                {
                    if (providedArguments.Any(a => ArgMatchesToken(a, token)))
                    {
                        yield return new ParseError
                        {
                            ErrorType = ErrorType.DuplicateArg,
                            ErrorToken = token,
                            Argument = token.Value
                        };
                    }
                    if (lastArg != null)
                    {
                        if (lastToken == lastArgToken && (lastArg.ValueMode == ArgumentMode.SingleValue || lastArg.ValueMode == ArgumentMode.MultiValue))
                        {
                            yield return new ParseError
                            {
                                ErrorType = ErrorType.MissingValue,
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
                        yield return new ParseError
                        {
                            ErrorType = ErrorType.UnknownArgument,
                            ErrorToken = token,
                            Argument = token.Value
                        };
                    }
                    else
                    {
                        providedArguments.Add(lastArg);
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
                                    yield return new ParseError
                                    {
                                        ErrorType = ErrorType.UnexpectedArgValue,
                                        ErrorToken = token,
                                        Argument = lastArgToken.Value
                                    };
                                }
                            }
                        }
                    }
                    else
                    {
                        yield return new ParseError
                        {
                            ErrorType = ErrorType.UnexpectedStartPositionalValue,
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
                        yield return new ParseError
                        {
                            ErrorType = ErrorType.MissingValue,
                            ErrorToken = lastArgToken,
                            Argument = lastArgToken.Value
                        };
                    }
                }
            }

            foreach (var rule in rules)
            {
                foreach (var error in rule.ValidateArguments(providedArguments))
                {
                    yield return error;
                }
            }
        }

        private static bool ArgMatchesToken(Argument argument, Token token)
        {
            return argument.LongName == token.Value || argument.ShortName + "" == token.Value;
        }
    }

    public class ParseError
    {
        public ErrorType ErrorType { get; set; }
        public Token ErrorToken { get; set; }
        public string Argument { get; set; }
    }

    public enum ErrorType
    {
        UnknownArgument,
        UnexpectedArgValue,
        UnexpectedStartPositionalValue,
        MissingValue,
        RequiredArgMissing,
        DuplicateArg
    }
}

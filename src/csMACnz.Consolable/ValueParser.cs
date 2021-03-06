﻿using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public class ValueParser
    {
        private readonly IRule[] _rules;

        public ValueParser(IRule[] rules)
        {
            _rules = rules;
        }

        public Dictionary<string, object> Parse(Token[] tokens)
        {
            var results = new Dictionary<string, object>();
            var arguments = RuleSet.GetValidArguments(_rules);
            foreach (var argument in arguments)
            {
                results[argument.LongName] = DefaultValue(argument);
            }

            Argument lastArg = null;
            foreach (var token in tokens)
            {
                switch (token.TokenType)
                {
                    case TokenType.Arg:
                        lastArg = arguments.SingleOrDefault(a => a.LongName == token.Value || a.ShortName.ToString() == token.Value);
                        if (lastArg.ValueMode == ArgumentMode.Flag)
                        {
                            results[lastArg.LongName] = true;
                            break;
                        }

                        break;
                    case TokenType.Value:
                        if (lastArg is null)
                        {
                            // TODO: what to do with positional args?
                        }
                        else if (lastArg.ValueMode == ArgumentMode.MultiValue)
                        {
                            ((List<string>)results[lastArg.LongName]).Add(token.Value);
                        }
                        else if (lastArg.ValueMode == ArgumentMode.SingleValue)
                        {
                            results[lastArg.LongName] = token.Value;
                        }

                        break;
                }
            }

            return results;
        }

        private static object DefaultValue(Argument argument)
        {
            switch (argument.ValueMode)
            {
                case ArgumentMode.Flag:
                    return false;
                case ArgumentMode.MultiValue:
                    return new List<string>();
                default:
                    return null;
            }
        }
    }
}

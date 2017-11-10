using System;
using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public static class Tokeniser
    {
        public static IEnumerable<Token> GetTokens(string[] args)
        {
            return args.SelectMany(Split);
        }

        private static IEnumerable<Token> Split(string arg)
        {
            bool isArg = false;
            bool isMultiArg = false;
            int index = 0;
            bool remainderIsValue = true;
            if (arg.StartsWith("--", StringComparison.Ordinal))
            {
                isArg = true;
                isMultiArg = false;
                index = arg.Length == 2 ? 1 : 2;
            }
            else if (arg.StartsWith("-", StringComparison.Ordinal) || arg.StartsWith("/", StringComparison.Ordinal))
            {
                isArg = true;
                isMultiArg = true;
                index = 1;
            }

            if (isArg)
            {
                remainderIsValue = false;
                bool hasSplitPoint = false;
                int splitIndex = int.MaxValue;

                if (arg.Contains("="))
                {
                    hasSplitPoint = true;
                    splitIndex = System.Math.Min(splitIndex, arg.IndexOf("=", StringComparison.Ordinal));
                }

                if (arg.Contains(":"))
                {
                    hasSplitPoint = true;
                    splitIndex = System.Math.Min(splitIndex, arg.IndexOf(":", StringComparison.Ordinal));
                }

                string argString;
                int rawIndex = index;
                if (hasSplitPoint)
                {
                    argString = arg.Substring(index, splitIndex - index);
                    index = splitIndex + 1;
                    remainderIsValue = true;
                }
                else
                {
                    argString = arg.Substring(index);
                }

                if (isMultiArg)
                {
                    foreach (int i in Enumerable.Range(0, argString.Length))
                    {
                        char c = argString[i];
                        yield return new Token(TokenType.Arg, c.ToString(), arg, rawIndex + i);
                    }
                }
                else
                {
                    yield return new Token(TokenType.Arg, argString, arg, rawIndex);
                }
            }

            if (remainderIsValue)
            {
                yield return new Token(TokenType.Value, arg.Substring(index), arg, index);
            }
        }
    }
}

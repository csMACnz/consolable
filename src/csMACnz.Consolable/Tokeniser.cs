﻿using System.Collections.Generic;
using System.Linq;

namespace csMACnz.Consolable
{
    public class Tokeniser
    {
        public IEnumerable<Token> GetTokens(string[] args)
        {    
            return args.SelectMany(Split);
        }

        private IEnumerable<Token> Split(string arg)
        {
            bool isArg = false;
            bool isMultiArg = false;
            int index = 0;
            bool remainderIsValue = true;
            if(arg.StartsWith("--"))
            {
                isArg=true;
                isMultiArg = false;
                if(arg.Length == 2)
                {
                    index = 1;
                }
                else
                {
                    index = 2;
                }
            }
            else if(arg.StartsWith("-") || arg.StartsWith("/"))
            {
                isArg=true;
                isMultiArg = true;
                index = 1;
            }

            if(isArg){
                remainderIsValue = false;
                bool hasSplitPoint = false;
                int splitIndex = int.MaxValue;
                
                if(arg.Contains("="))
                {
                    hasSplitPoint = true;
                    splitIndex = System.Math.Min(splitIndex, arg.IndexOf("="));
                }
                if(arg.Contains(":"))
                {
                    hasSplitPoint = true;
                    splitIndex = System.Math.Min(splitIndex, arg.IndexOf(":"));
                }

                string argString;
                if(hasSplitPoint)
                {
                    argString = arg.Substring(index, splitIndex-index);
                    index=splitIndex + 1;
                    remainderIsValue = true;
                }
                else
                {
                    argString = arg.Substring(index);
                }

                if(isMultiArg)
                {
                    foreach(char c in argString)
                    {
                        yield return new Token(TokenType.Arg, c.ToString());
                    }
                }
                else
                {
                    yield return new Token(TokenType.Arg, argString);
                }
            }

            if(remainderIsValue)
            {
                yield return new Token(TokenType.Value, arg.Substring(index));
            }
        }
    }

    public class Token{
        public Token(TokenType type, string value)
        {
            TokenType = type;
            Value = value;
        }
        public TokenType TokenType { get; }
        public string Value { get; }
     }

    public enum TokenType
    {
        Value,
        Arg
    }
}

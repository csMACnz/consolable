using System.Collections.Generic;
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
            int index = 0;
            bool remainderIsValue = true;
            if(arg.StartsWith("--"))
            {
                isArg=true;
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

                if(hasSplitPoint)
                {
                    yield return new Token(TokenType.Arg, arg.Substring(index, splitIndex-index));
                    index=splitIndex + 1;
                    remainderIsValue = true;
                }
                else
                {
                    yield return new Token(TokenType.Arg, arg.Substring(index));
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

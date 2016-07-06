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
            if(arg.StartsWith("-")){
                yield return new Token(TokenType.Arg, arg.Substring(1));
            }
            else{
                yield return new Token(TokenType.Value, arg);
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

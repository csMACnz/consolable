namespace csMACnz.Consolable
{
    public class Token
    {
        public Token(TokenType type, string value, string raw, int rawIndex)
        {
            TokenType = type;
            Value = value;
            Raw = raw;
            RawIndex = rawIndex;
        }

        public TokenType TokenType { get; }

        public string Value { get; }

        public string Raw { get; }

        public int RawIndex { get; }
     }
}

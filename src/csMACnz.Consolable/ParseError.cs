namespace csMACnz.Consolable
{
    public class ParseError
    {
        public ErrorType ErrorType { get; set; }

        public Token ErrorToken { get; set; }

        public string Argument { get; set; }
    }
}

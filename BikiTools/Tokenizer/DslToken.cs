
namespace BikiTools.Tokenizer
{
    public class DslToken
    {
        public TokenType TokenType;
        public string Value { get; set; }

        public DslToken(TokenType tokenType, string value = "")
        {
            TokenType = tokenType;
            Value = value;
        }

        public DslToken Clone()
        {
            return new DslToken(TokenType, Value);
        }

    }
}
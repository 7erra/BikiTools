using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BikiTools.Tokenizer
{
    public class TokenDefinition
    {
        private readonly Regex _regex;
        private readonly TokenType _returnsToken;
        public readonly int Priority;

        public TokenDefinition(TokenType returnsToken, Regex regex, int precedence = 0)
        {
            _regex = regex;
            _returnsToken = returnsToken;
            Priority = precedence;
        }

        public IEnumerable<TokenMatch> FindMatches(string inputString)
        {
            MatchCollection matches = _regex.Matches(inputString);
            for (int i = 0; i < matches.Count; i++)
            {
                Match currentMatch = matches[i];
                yield return new TokenMatch()
                {
                    StartIndex = currentMatch.Index,
                    EndIndex = currentMatch.Index + currentMatch.Length,
                    TokenType = _returnsToken,
                    Value = currentMatch.Value,
                    Precedence = Priority
                };
            }
        }
    }
}

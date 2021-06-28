using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace BikiTools.Tokenizer
{
    public class Tokenizer
    {
        #region Fields
        public static readonly Regex RegexUnkown = new(".");
        public static readonly Regex RegexCommand = new(GenerateCommandRegex());
        public static readonly Regex RegexComment = new(@"//.*?(?=\r?\n)");
        public static readonly Regex RegexWhitespace = new(@"\s+");
        public static readonly Regex RegexSemicolon = new(";");
        public static readonly Regex RegexNumber = new(@"\b\d+(?:\.\d+)?|\.\d+\b");
        public static readonly Regex RegexString = new("\".*\"");
        public static readonly Regex RegexBIFunction = new(@"\bBIS_fnc_\w+", RegexOptions.IgnoreCase);
        public static readonly Regex RegexLocalVariable = new(@"\b_\w+");
        public static readonly Regex RegexCommentMultiLine = new(@"\/\*.*?\*\/", RegexOptions.Singleline);

        private readonly List<TokenDefinition> _tokenDefinitions = new()
        {
            new TokenDefinition(TokenType.Unkown, RegexUnkown, int.MaxValue),
            new TokenDefinition(TokenType.Command, RegexCommand),
            new TokenDefinition(TokenType.Comment, RegexComment, 2),
            new TokenDefinition(TokenType.Whitespace, RegexWhitespace),
            new TokenDefinition(TokenType.Semicolon, RegexSemicolon),
            new TokenDefinition(TokenType.Number, RegexNumber),
            new TokenDefinition(TokenType.String, RegexString, 3),
            new TokenDefinition(TokenType.BIFunction, RegexBIFunction, 2), // higher than string to link function even when in string
            new TokenDefinition(TokenType.LocalVariable, RegexLocalVariable),
            new TokenDefinition(TokenType.CommentMultiLine, RegexCommentMultiLine, 2)
        };

        public const string CommandsFile = "sqf\\commands.txt";
        #endregion // Fields

        #region Methods
        public static string GenerateCommandRegex()
        {
            string[] commands = File.ReadAllLines(CommandsFile);
            string regexCommand = "\\b(" + string.Join('|', commands) + ")\\b";
            return regexCommand;
        }

        private List<TokenMatch> FindTokenMatches(string code)
        {
            List<TokenMatch> tokenMatches = new();
            foreach (TokenDefinition tokenDefinition in _tokenDefinitions)
            {
                tokenMatches.AddRange(tokenDefinition.FindMatches(code).ToList());
            }
            return tokenMatches;
        }

        public IEnumerable<DslToken> Tokenize(string code)
        {
            List<TokenMatch> tokenMatches = FindTokenMatches(code);
            List<IGrouping<int, TokenMatch>> groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                .OrderBy(x => x.Key)
                .ToList();
            TokenMatch lastMatch = null;
            for (int i = 0; i < groupedByIndex.Count; i++)
            {
                TokenMatch bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
                if (bestMatch.StartIndex < lastMatch?.EndIndex)
                {
                    continue;
                }
                yield return new DslToken(bestMatch.TokenType, bestMatch.Value);
                lastMatch = bestMatch;
            }
        }

        public string Untokenize(IEnumerable<DslToken> tokens)
        {
            return string.Join("", tokens.Select(x => x.Value));
        }
        #endregion Methods
    }
}

using Microsoft.Toolkit.Mvvm.ComponentModel;
using BikiTools.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BikiTools.ViewModel
{
    public class BikiCodeViewModel : ObservableObject
    {
        public enum RadioButtons
        {
            None,
            CodeTag,
            SpaceIndent
        }

        private string input;
        public string Input
        {
            get => input;
            set
            {
                // Tokenize
                BikiCodeTokenizer tokenizer = new(value);
                List<DslToken> newTokens = new();
                foreach (DslToken t in tokenizer.Tokens)
                {
                    if (t.TokenType == TokenType.Command || t.TokenType == TokenType.BIFunction)
                    {
                        t.Value = "[[" + t.Value + "]]";
                    }
                    else if (t.TokenType == TokenType.Comment)
                    {
                        t.Value = "{{cc|" + t.Value.Replace("//", "").Trim() + "}}";
                    }
                    newTokens.Add(t);
                }
                string codeFormatted = BikiCodeTokenizer.Untokenize(newTokens);
                codeFormatted = Regex.Replace(codeFormatted, @"^", " ", RegexOptions.Multiline);
                Output = codeFormatted;

                SetProperty(ref input, value);
            }
        }

        private string output;
        public string Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }
    }
}

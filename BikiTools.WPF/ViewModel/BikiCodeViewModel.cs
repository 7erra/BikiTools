using Microsoft.Toolkit.Mvvm.ComponentModel;
using BikiTools.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BikiTools.ViewModel
{
    public enum HighlightOptions
    {
        None,
        CodeTag,
        SpaceIndent
    }

    public class BikiCodeViewModel : ObservableObject
    {

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
                    if (t.TokenType is TokenType.Command or TokenType.BIFunction)
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
                if (SelectedHighlightOption == HighlightOptions.SpaceIndent)
                {
                    codeFormatted = Regex.Replace(codeFormatted, @"^", " ", RegexOptions.Multiline);
                }
                else if (SelectedHighlightOption == HighlightOptions.CodeTag)
                {
                    codeFormatted = "<code>" + codeFormatted + "</code>";
                }
                Output = codeFormatted;

                _ = SetProperty(ref input, value);
            }
        }

        private string output;
        public string Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }

        private HighlightOptions selectedHighlightOption;

        public HighlightOptions SelectedHighlightOption
        {
            get => selectedHighlightOption;
            set
            {
                SetProperty(ref selectedHighlightOption, value);
            }
        }
    }
}

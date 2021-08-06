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
                bool changed = SetProperty(ref input, value);
                if (changed) OnPropertyChanged(nameof(Output));
            }
        }

        public string Output
        {
            get
            {
                // Tokenize
                if (Input == null) return "";
                BikiCodeTokenizer tokenizer = new(Input);
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
                    else if (t.TokenType == TokenType.EqualSign && UseEqualSignTemplate)
                    {
                        t.Value = "{{" + t.Value + "}}";
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
                return codeFormatted;
            }
        }

        private HighlightOptions selectedHighlightOption;

        public HighlightOptions SelectedHighlightOption
        {
            get => selectedHighlightOption;
            set
            {
                bool changed = SetProperty(ref selectedHighlightOption, value);
                if (changed) OnPropertyChanged(nameof(Output));
            }
        }

        private bool useEqualSignTemplate;

        public bool UseEqualSignTemplate
        {
            get => useEqualSignTemplate;
            set
            {
                bool changed = SetProperty(ref useEqualSignTemplate, value);
                if (changed) OnPropertyChanged(nameof(Output));
            }
        }

    }
}

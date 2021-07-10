using BikiTools.Tokenizer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                if (SetProperty(ref input, value))
                {
                    OnPropertyChanged(nameof(Output));
                }
            }
        }

        /// <summary>
        /// Formatted input with added Mediawiki highlighting.
        /// </summary>
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
                        t.Value = $"[[{t.Value}]]";
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
                return codeFormatted;
            }
        }

        private HighlightOptions selectedHighlightOption;

        public HighlightOptions SelectedHighlightOption
        {
            get => selectedHighlightOption;
            set
            {
                if (SetProperty(ref selectedHighlightOption, value))
                {
                    OnPropertyChanged(nameof(Output));
                }
            }
        }
    }
}

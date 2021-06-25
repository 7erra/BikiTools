using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikiTools.Tokenizer
{
    public enum TokenType
    {
        Unkown,
        Command,
        Comment,
        Whitespace,
        Semicolon,
        Number
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BikiTools.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using static System.Reflection.MethodBase;

namespace BikiTools.Tokenizer.Tests
{
    /// <summary>
    /// The class used to test the functionality of the Tokenizer class
    /// </summary>
    /// <example>
    /// Set up a test as follows:
    /// 1) Create a test method that starts with "Tokenize_", eg "Tokenize_SimpleOneliner"
    /// 2) Create an sqf file named like this:  BikiTools.Tests\scripts\SimpleOneliner.sqf
    /// 3) Run the test with the <c>TestExampleScript</c> method
    /// For examples see below
    /// </example>
    [TestClass()]
    public class TokenizerTests
    {
        #region Values
        private static int TokenTextMaxWidth
        {
            get
            {
                Array values = Enum.GetValues(typeof(TokenType));
                int max = "Token".Length;
                foreach (var v in values)
                {
                    max = Math.Max(v.ToString().Length, max);
                }
                return 1;
            } 
        }
        #endregion
        #region Helpers
        private static string LoadSqfFile(string file)
        {
            return File.ReadAllText($"scripts\\{file}.sqf");
        }
        private static string LoadSqfFile(MethodBase method)
        {
            string file = method.Name["Tokenize_".Length..];
            return LoadSqfFile(file);
        }
        private static void LogActualTokens(IEnumerable<DslToken> tokens)
        {
            Debug.WriteLine("\n### Actual Tokens ###");
            Debug.WriteLine($"{"Token",15} | Value");
            foreach (DslToken token in tokens)
            {
                Debug.WriteLine(string.Format("{0,-15} | \"{1}\"", token.TokenType, token.Value.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t")));
            }
        }
        private static void TestExampleScript(MethodBase method, List<TokenType> expectedTokens = null)
        {
            string code = LoadSqfFile(method);
            Debug.WriteLine("### Input ###");
            Debug.WriteLine(code);
            BikiCodeTokenizer tokenizer = new(code);
            IEnumerable<DslToken> tokens = tokenizer.Tokenize(code);
            LogActualTokens(tokens);
            if (expectedTokens != null)
            {
                // Compare the received tokens
                CollectionAssert.AreEqual(expectedTokens, tokens.Select(x => x.TokenType).ToList());
            }
            string output = tokenizer.Untokenize();
            Debug.WriteLine("\n### Output ###");
            Debug.WriteLine(output);
            string[] inputLines = code.Split('\n');
            string[] outputLines = output.Split('\n');
            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines[i].Length; j++)
                {
                    char charInput = inputLines[i][j];
                    char charOutput;
                    try
                    {
                        charOutput = outputLines.ElementAtOrDefault(i).ElementAtOrDefault(j);
                    }
                    catch (ArgumentNullException)
                    {
                        continue;
                    }
                    Assert.AreEqual(charInput, charOutput, $"Line: {i + 1}, Char: {j + 1} (Code in: {(int)charInput} | Code out: {(int)charOutput})");
                }
            }
            Assert.AreEqual(code, output);
        }
        #endregion

        #region Meta Tests
        [TestMethod()]
        public void GenerateCommandRegexTest()
        {
            // Arrange
            string commandRegex = BikiCodeTokenizer.GenerateCommandRegex();
            // Act
            // Assert
            Debug.WriteLine(commandRegex);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void Regex_Commands()
        {
            Regex r = BikiCodeTokenizer.RegexCommand;
            string input = File.ReadAllText(BikiCodeTokenizer.CommandsFile);
            int lineCount = File.ReadAllLines(BikiCodeTokenizer.CommandsFile).Length;

            var matches = r.Matches(input);

            Assert.IsTrue(matches.Count == lineCount);
        }
        #endregion

        #region Tokenizer Tests
        [TestMethod()]
        public void Tokenize_SimpleOneliner()
        {
            TestExampleScript(GetCurrentMethod());
        }

        [TestMethod()]
        public void Tokenize_SimpleOneLinerComment()
        {
            List<TokenType> expectedTokenTypes = new()
            {
                TokenType.Command,
                TokenType.Whitespace,
                TokenType.Command,
                TokenType.Whitespace,
                TokenType.Number,
                TokenType.Semicolon,
                TokenType.Whitespace,
                TokenType.Comment
            };
            TestExampleScript(GetCurrentMethod(), expectedTokenTypes);
        }
        [TestMethod()]
        public void Tokenize_Whitespace()
        {
            TestExampleScript(GetCurrentMethod());
        }
        [TestMethod()]
        public void Tokenize_uiScriptExample()
        {
            TestExampleScript(GetCurrentMethod());
        }
        [TestMethod()]
        public void Tokenize_GeneralDeletion()
        {
            TestExampleScript(GetCurrentMethod());
        }
        #endregion
    }
}

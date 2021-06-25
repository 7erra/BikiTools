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
    /// 3) Test the Tokenizer with the GetActualTokens method
    /// 4) Manually create a list of correct Tokens
    /// 5) Compare
    /// For examples see below
    /// </example>
    [TestClass()]
    public class TokenizerTests
    {
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
        private static void LogActualTokens(List<TokenType> tokens)
        {
            Debug.WriteLine("### Actual Tokens ###");
            foreach (TokenType token in tokens)
            {
                Debug.WriteLine(token);
            }
        }
        private static List<TokenType> GetActualTokens(MethodBase method)
        {
            string code = LoadSqfFile(method.Name["Tokenize_".Length..]);
            Tokenizer tokenizer = new();
            List<TokenType> actualTokens = tokenizer.Tokenize(code).Select(x => x.TokenType).ToList();
            LogActualTokens(actualTokens);
            return actualTokens;
        }
        private void TestExampleScript(MethodBase method)
        {
            string code = LoadSqfFile(method);
            Tokenizer tokenizer = new();
            string output = tokenizer.Untokenize(tokenizer.Tokenize(code));
            Assert.AreEqual(code, output);
        }
        #endregion

        #region Meta Tests
        [TestMethod()]
        public void GenerateCommandRegexTest()
        {
            // Arrange
            string commandRegex = Tokenizer.GenerateCommandRegex();
            // Act
            // Assert
            Debug.WriteLine(commandRegex);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void TokenizerRegex_Commands()
        {
            Regex r = Tokenizer.RegexCommand;
            string input = File.ReadAllText(Tokenizer.CommandsFile);
            int lineCount = File.ReadAllLines(Tokenizer.CommandsFile).Length;

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
            TestExampleScript(GetCurrentMethod());
        }
        #endregion
    }
}

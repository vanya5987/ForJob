using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            List<Token> actualResult = FieldsParserTask.ParseLine(input);

            Assert.AreEqual(expectedResult.Length, actualResult.Count);

            for (int i = 0; i < expectedResult.Length; ++i)
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("", new string[] { })]
        [TestCase("    text", new[] { "text" })]
        [TestCase(@"""a 'b' 'c' d""", new[] { "a 'b' 'c' d" })]
        [TestCase(@"'""1"" ""2"" ""3""'", new[] { @"""1"" ""2"" ""3""" })]
        [TestCase(@"'x y'", new[] { "x y" })]
        [TestCase(@"a""b", new[] { "a", "b" })]
        [TestCase(@"""a \""c\""""", new[] { @"a ""c""" })]
        [TestCase(@"""\\""", new[] { @"\" })]
        [TestCase(@"'", new[] { @"" })]
        [TestCase("hello    world", new[] { "hello", "world" })]
        [TestCase(@"'\''", new[] { @"'" })]
        [TestCase(@"'abc' a", new[] { @"abc", "a" })]
        [TestCase(@"'abc ", new[] { @"abc " })]

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            List<Token> resultList = new List<Token>();

            for (int i = 0; i < line.Length; i++)
                if ((line[i] == '\'') || (line[i] == '\"'))
                {
                    resultList.Add(QuotedFieldTask.ReadQuotedField(line, i));
                    i = resultList[resultList.Count - 1].GetIndexNextToToken() - 1;
                }
                else
                    if (line[i] != ' ')
                {
                    resultList.Add(RemoveSpace(line, i));
                    i = resultList[resultList.Count - 1].GetIndexNextToToken() - 1;
                }

            return resultList;
        }

        private static Token RemoveSpace(string line, int index)
        {
            string actualValue = "";
            int length = 0;

            for (int i = index; i < line.Length; i++)
            {
                if ((line[i] == ' ') || (line[i] == '\'') || (line[i] == '\"'))
                    break;

                actualValue += line[i];
                lenght++;
            }
            return new Token(actualValue, index, lenght);
        }

        private static Token ReadField(string line, int startIndex)
        {
            return new Token(line, 0, line.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}
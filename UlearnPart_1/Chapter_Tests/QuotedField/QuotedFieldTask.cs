using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            Token actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            int actualIndex = startIndex + 1;
            string actualValue = "";

            while (actualIndex < line.Length)
            {
                if (line[actualIndex] == line[startIndex])
                    break;
                if (line[actualIndex] == '\\')
                    actualIndex++;
                actualValue += line[actualIndex];
                actualIndex++;
            }

            if ((actualIndex >= line.Length))
                actualIndex--;

            return new Token(actualValue, startIndex, actualIndex - startIndex + 1);
        }
    }
}
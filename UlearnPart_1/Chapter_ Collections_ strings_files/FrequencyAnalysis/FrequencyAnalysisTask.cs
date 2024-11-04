namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> subResult = new Dictionary<string, string>();

            for (int i = 0; i < text.Count; i++)
                subResult = GetDictonarySentence(text[i], subResult);

            foreach (var bigram in subResult)
                result.Add(bigram.Key, GetHighestValue(bigram.Value));

            return result;
        }

        private static string GetHighestValue(string value)
        {
            Dictionary<string, int> wordDictonary = new Dictionary<string, int>();
            string[] arrWords = value.Split(' ');

            foreach (string word in arrWords)
                if (wordDictonary.ContainsKey(word))
                    wordDictonary[word] = wordDictonary[word] + 1;
                else
                    wordDictonary[word] = 1;

            string highestValue = SearchHighestValue(wordDictonary);

            return highestValue;
        }

        private static string SearchHighestValue(Dictionary<string, int> wordDictonary)
        {
            string highestValue = " ";
            int maxValue = 0;

            foreach (var word in wordDictonary)
                if (word.Value > maxValue)
                {
                    maxValue = word.Value;
                    highestValue = word.Key;
                }
                else
                    if ((word.Value == maxValue) && (String.CompareOrdinal(word.Key, highestValue) < 0))
                    highestValue = word.Key;

            return highestValue;
        }

        private static Dictionary<string, string> GetDictonarySentence(List<string> list, Dictionary<string, string> dictonarySentence)
        {
            dictonarySentence = GetBigram(list, dictonarySentence);
            dictonarySentence = GetThreegram(list, dictonarySentence);

            return dictonarySentence;
        }

        private static Dictionary<string, string> GetThreegram(List<string> list, Dictionary<string, string> dictonarySentence)
        {
            for (int i = 0; i < list.Count - 2; i++)
                if (dictonarySentence.ContainsKey(list[i] + " " + list[i + 1]))
                    dictonarySentence[list[i] + " " + list[i + 1]] += " " + list[i + 2];
                else
                    dictonarySentence.Add(list[i] + " " + list[i + 1], list[i + 2]);

            return dictonarySentence;
        }

        private static Dictionary<string, string> GetBigram(List<string> list, Dictionary<string, string> dictonarySentence)
        {
            for (int i = 0; i < list.Count - 1; i++)
                if (dictonarySentence.ContainsKey(list[i]))
                    dictonarySentence[list[i]] += " " + list[i + 1];
                else
                    dictonarySentence.Add(list[i], list[i + 1]);

            return dictonarySentence;
        }
    }
}
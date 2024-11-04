namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            string[] sentences = text.Split(new char[] { '.', '?', '!', ':', ';', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);

            string[][] words = new string[sentences.Length][];

            char[] punctuationMarks = new char[] {
                ' ',' ', ',', '/', '—', '“', '”',
                '‘', '…', '^', '#', '$', '-', '+',
                '1', '=', '\t', '\n', '\r', '\"',
                '2','3','4','5','6','7','8','9','0', '*'};

            for (int i = 0; i < sentences.Length; i++)
                words[i] = sentences[i].Split(punctuationMarks, StringSplitOptions.RemoveEmptyEntries);

            List<List<string>> sentencesList = MakeSentencesList(sentences, words);

            return sentencesList;
        }

        private static List<List<string>> MakeSentencesList(string[] sentences, string[][] words)
        {
            List<List<string>> sentencesList = new List<List<string>>();
            List<string> wordsList = new List<string>();

            for (int i = 0; i < sentences.Length; i++)
            {
                wordsList = new List<string>();

                for (int j = 0; j < words[i].Length; j++)
                {
                    words[i][j].Replace(" ", "");
                    wordsList.Add(words[i][j].ToLower());
                }

                if (wordsList.Count != 0)
                    sentencesList.Add(wordsList);
            }

            return sentencesList;
        }
    }
}
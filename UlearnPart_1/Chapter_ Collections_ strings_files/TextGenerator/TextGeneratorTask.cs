namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
            int phraseLength = 1;
            string[] phraseBeginningArr = phraseBeginning.Split(' ');
            string wordKeyFirst = phraseBeginning;
            string wordKeyNext = phraseBeginning;
            string subWordKeyNext = phraseBeginning;

            if (phraseBeginningArr.Length > 1)
            {
                wordKeyFirst = phraseBeginningArr[phraseBeginningArr.Length - 2];
                wordKeyNext = phraseBeginningArr[phraseBeginningArr.Length - 1];
            }

            string[] keyArr = new string[] { wordKeyFirst, wordKeyNext, subWordKeyNext };

            phraseBeginning = MakePhraseBeginning(wordKeyFirst, wordKeyNext, wordsCount,
                phraseLength, nextWords, subWordKeyNext, phraseBeginning, phraseBeginningArr, keyArr);


            return phraseBeginning;
        }

        private static string MakePhraseBeginning(string wordKeyFirst, string wordKeyNext, int wordsCount,
            int phraseLength, Dictionary<string, string> nextWords, string subWordKeyNext, string phraseBeginning,
            string[] phraseBeginningArr, string[] keyArr)
        {
            while (true)
            {
                if (wordsCount < 1)
                    break;

                if (((phraseLength > 1) || (phraseBeginningArr.Length > 1))
                    && (nextWords.ContainsKey(keyArr[0] + " " + keyArr[1])))
                {
                    phraseBeginning += " " + nextWords[keyArr[0] + " " + keyArr[1]];
                    keyArr = StepKeyThree(keyArr, nextWords);
                    wordsCount--;
                }
                else
                {
                    if (nextWords.ContainsKey(keyArr[1]))
                    {
                        phraseBeginning += " " + nextWords[keyArr[1]];
                        keyArr = StepKeyBee(keyArr, nextWords);
                        wordsCount--;
                        phraseLength++;
                    }
                    else
                        return phraseBeginning;
                }
            }
            return phraseBeginning;
        }

        private static string[] StepKeyBee(string[] keyArr, Dictionary<string, string> nextWords)
        {
            keyArr[2] = keyArr[1];
            keyArr[1] = nextWords[keyArr[1]];
            keyArr[0] = keyArr[2];
            return keyArr;
        }

        private static string[] StepKeyThree(string[] keyArr, Dictionary<string, string> nextWords)
        {
            keyArr[2] = keyArr[1];
            keyArr[1] = nextWords[keyArr[0] + " " + keyArr[1]];
            keyArr[0] = keyArr[2];
            return keyArr;
        }
    }
}
namespace Passwords;

public class CaseAlternatorTask
{
    public static List<string> AlternateCharCases(string lowercaseWord)
    {
        List<string> result = new List<string>();
        AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
        return result;
    }

    static void AlternateCharCases(char[] word, int startIndex, List<string> result)
    {
        if (startIndex >= word.Length)
        {
            result.Add(new string(word));
            return;
        }

        if (char.IsLetter(word[startIndex]))
        {
            word[startIndex] = char.ToLower(word[startIndex]);
            AlternateCharCases(word, startIndex + 1, result);

            word[startIndex] = char.ToUpper(word[startIndex]);
            if (char.IsUpper(word[startIndex]))
                AlternateCharCases(word, startIndex + 1, result);
        }
        else
            AlternateCharCases(word, startIndex + 1, result);
    }
}

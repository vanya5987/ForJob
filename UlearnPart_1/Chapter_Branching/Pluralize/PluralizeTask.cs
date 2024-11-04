namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        string startPhrase = "рубл";
        string result = string.Empty;

        int lastDigit = count % 10;
        int lastTwoDigits = count % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
            result = "ей";
        else if (lastDigit == 1)
            result = "ь";
        else if (lastDigit >= 2 && lastDigit <= 4)
            result = "я";
        else
            result = "ей";

        return string.Concat(startPhrase, result);
    }
}
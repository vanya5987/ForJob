namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        string startPhrase = "����";
        string result = string.Empty;

        int lastDigit = count % 10;
        int lastTwoDigits = count % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
            result = "��";
        else if (lastDigit == 1)
            result = "�";
        else if (lastDigit >= 2 && lastDigit <= 4)
            result = "�";
        else
            result = "��";

        return string.Concat(startPhrase, result);
    }
}
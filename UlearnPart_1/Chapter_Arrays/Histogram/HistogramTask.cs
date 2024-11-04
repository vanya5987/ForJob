using System;

namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        string[] days = new string[31];

        for (int day = 1; day < 32; day++)
            days[day - 1] = day.ToString();

        double[] birthsCounts = new double[31];

        foreach (var person in names)
            if ((name == person.Name) && (person.BirthDate.Day != 1))
                birthsCounts[person.BirthDate.Day - 1]++;

        return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name), days, birthsCounts);
    }
}
using System;

namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        string[] days = new string[30];
        string[] months = new string[12];
        double[,] heat = new double[30, 12];

        foreach (var name in names)
            if (name.BirthDate.Day != 1)
                heat[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;

        for (int day = 2; day < 32; day++)
            days[day - 2] = day.ToString();

        for (int month = 1; month < 13; month++)
            months[month - 1] = month.ToString();

        return new HeatmapData("Пример карты интенсивностей", heat, days, months);
    }
}
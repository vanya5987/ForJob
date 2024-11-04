using System;
using System.Collections.Generic;

namespace Recognizer;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        List<double> result = new List<double>();

        int xLength = original.GetLength(0);
        int yLength = original.GetLength(1);

        int whitePixelsCount = (int)(original.Length * whitePixelsFraction);

        for (int x = 0; x < xLength; x++)
            for (int y = 0; y < yLength; y++)
                result.Add(original[x, y]);

        result.Sort();
        result.Reverse();
        result.RemoveRange(whitePixelsCount, result.Count - whitePixelsCount);

        for (int x = 0; x < xLength; x++)
            for (int y = 0; y < yLength; y++)
                if (result.Contains(original[x, y]))
                    original[x, y] = 1;
                else
                    original[x, y] = 0;

        return original;
    }
}

using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double[,] MedianFilter(double[,] original)
        {
            int rows = original.GetLength(0);
            int cols = original.GetLength(1);
            double[,] result = new double[rows, cols];

            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                {
                    List<double> neighbors = GetNeighbors(original, x, y);
                    neighbors.Sort();
                    result[x, y] = GetMedian(neighbors);
                }

            return result;
        }

        private static List<double> GetNeighbors(double[,] array, int x, int y)
        {
            List<double> neighbors = new List<double>();

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                        neighbors.Add(array[newX, newY]);
                }

            return neighbors;
        }

        private static double GetMedian(List<double> values)
        {
            int count = values.Count;

            if (count % 2 == 1)
                return values[count / 2];
            else
                return (values[count / 2 - 1] + values[count / 2]) / 2.0;
        }
    }
}
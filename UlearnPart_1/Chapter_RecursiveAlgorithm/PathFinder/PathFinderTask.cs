using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static double BestLength = 0;

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            if (checkpoints.Length > 1)
            {
                for (int i = 0; i < checkpoints.Length - 1; i++)
                {
                    BestLength += PointExtensions.DistanceTo(checkpoints[i], checkpoints[i + 1]);
                }

                List<int[]> result = new List<int[]>();
                MakeTrivialPermutation(1, new int[checkpoints.Length], result, checkpoints);

                int[] bestOrder = result[result.Count - 1];
                return bestOrder;
            }
            if (checkpoints.Length == 1)
                return new int[1];
            else
                return new int[0];
        }

        private static void MakeTrivialPermutation(int position, int[] permutation,
                                                    List<int[]> result, Point[] checkpoints)
        {
            int size = permutation.Length;
            double length = CountLength(position, checkpoints, permutation);

            if (length > BestLength)
                return;

            if (position == permutation.Length)
            {
                AddResultPermutation(permutation, result, length);

                return;
            }

            for (int i = 1; i < size; i++)
            {
                int index = Array.IndexOf(permutation, i, 1, position - 1);
                if (index != -1)
                    continue;
                permutation[position] = i;
                MakeTrivialPermutation(position + 1, permutation, result, checkpoints);
            }

            return;
        }

        private static void AddResultPermutation(int[] permutation, List<int[]> result, double length)
        {
            int[] subRes = new int[permutation.Length];

            for (int i = 1; i < permutation.Length; i++)
                subRes[i] = permutation[i];

            result.Add(subRes);
            BestLength = length;
        }

        private static double CountLength(int position, Point[] checkpoints, int[] permutation)
        {
            double length = 0.0;

            for (int i = 1; i < position; i++)
                length += PointExtensions.DistanceTo(checkpoints[permutation[i - 1]], checkpoints[permutation[i]]);

            return length;
        }
    }
}
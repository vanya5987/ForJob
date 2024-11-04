using System;

namespace Fractals;

internal static class DragonFractalTask
{
    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        Random random = new Random(seed);

        double x = 1.0;
        double y = 0.0;

        DrawFractal(random, x, y, iterationsCount, pixels);
    }

    private static void DrawFractal(Random random, double directionX, double directionY, int iterationsCount, Pixels pixels)
    {
        double tempX = directionX;
        double tempY = directionY;
        for (; iterationsCount > 0; iterationsCount--)
        {
            var nextNumber = random.Next(2);

            if (nextNumber == 0)
            {
                tempX = (directionX * Math.Cos(45 * Math.PI / 180) - directionY * Math.Sin(45 * Math.PI / 180)) / Math.Sqrt(2);
                tempY = (directionX * Math.Sin(45 * Math.PI / 180) + directionY * Math.Cos(45 * Math.PI / 180)) / Math.Sqrt(2);
            }
            else
            {
                tempX = (directionX * Math.Cos(135 * Math.PI / 180) - directionY * Math.Sin(135 * Math.PI / 180)) / Math.Sqrt(2) + 1;
                tempY = (directionX * Math.Sin(135 * Math.PI / 180) + directionY * Math.Cos(135 * Math.PI / 180)) / Math.Sqrt(2);
            }

            directionX = tempX;
            directionY = tempY;
            pixels.SetPixel(tempX, tempY);
        }
    }
}
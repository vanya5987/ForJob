using System;

namespace AngryBirds;

public static class AngryBirdsTask
{
    private const double Gravity = 9.8;

    public static double FindSightAngle(double velocity, double distance)
    {
        double angleSinValue = Gravity * distance / Math.Pow(velocity, 2);
        double aimingAngle = Math.Asin(angleSinValue) / 2;

        if (angleSinValue < 0 || angleSinValue > 1)
            throw new OutOfMemoryException(nameof(angleSinValue));

        return aimingAngle;
    }
}
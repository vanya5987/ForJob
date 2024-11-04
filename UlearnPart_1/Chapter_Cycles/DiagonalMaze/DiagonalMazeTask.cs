using System.Drawing;

namespace Mazes;

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        int stepCount = 3;

        if (height >= width)
            MoveOutHeight(robot, height, width, stepCount);
        else
            MoveOutWidth(robot, height, width, stepCount);
    }

    private static void MoveOutWidth(Robot robot, int height, int width, int stepCount)
    {
        for (int i = 0; i < height - stepCount; i++)
        {
            MoveRight(robot, width - stepCount, height - stepCount);
            MoveDown(robot, height - stepCount, width - stepCount);
        }

        MoveRight(robot, width - stepCount, height - stepCount);
    }

    private static void MoveOutHeight(Robot robot, int height, int width, int stepCount)
    {
        for (int i = 0; i < width - stepCount; i++)
        {
            MoveDown(robot, height - stepCount, width - stepCount);
            MoveRight(robot, width - stepCount, height - stepCount);
        }

        MoveDown(robot, height - stepCount, width - stepCount);
    }

    private static void MoveDown(Robot robot, int height, int width)
    {
        if (height > width)
            for (int j = 0; j < height / width; j++)
                robot.MoveTo(Direction.Down);
        else
            robot.MoveTo(Direction.Down);
    }

    private static void MoveRight(Robot robot, int width, int height)
    {
        if (width > height)
            for (int j = 0; j < width / height; j++)
                robot.MoveTo(Direction.Right);
        else
            robot.MoveTo(Direction.Right);
    }
}
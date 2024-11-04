namespace Mazes;

public static class EmptyMazeTask
{
    public static void MoveOut(Robot robot, int frameWidth, int frameHeight)
    {
        int frameBoardCount = 3;

        while (!robot.Finished)
        {
            MoveDown(robot, frameHeight - frameBoardCount);
            MoveRight(robot, frameWidth - frameBoardCount);
        }
    }

    public static void MoveRight(Robot robot, int mazeTileWidth)
    {
        for (int i = 0; i < mazeTileWidth; i++)
                robot.MoveTo(Direction.Right);
    }

    public static void MoveDown(Robot robot, int mazeTileHeight)
    {
        for (int i = 0; i < mazeTileHeight; i++)
                robot.MoveTo(Direction.Down);
    }
}
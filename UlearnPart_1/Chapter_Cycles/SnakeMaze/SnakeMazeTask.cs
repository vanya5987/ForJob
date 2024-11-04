namespace Mazes;

public static class SnakeMazeTask
{
    public static void MoveOut(Robot robot, int frameWidth, int frameHeight)
    {
        int horizontalBoardsWithPlayer = 3;
        int verticalBoardsWithPlayer = 2;

        while (!robot.Finished)
        {
            MoveRight(robot, frameWidth - horizontalBoardsWithPlayer);
            MoveDown(robot, verticalBoardsWithPlayer);
            MoveLeft(robot, frameWidth - horizontalBoardsWithPlayer);

            if (robot.Finished)
                break;

            MoveDown(robot, verticalBoardsWithPlayer);
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

    public static void MoveLeft(Robot robot, int mazeTileWidth)
    {
        for (int i = 0; i < mazeTileWidth; i++)
            robot.MoveTo(Direction.Left);
    }
}
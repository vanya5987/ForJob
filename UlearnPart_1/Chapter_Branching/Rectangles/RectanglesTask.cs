using System;

namespace Rectangles;

public static class RectanglesTask
{
    public static bool IsInsideLeft(Rectangle r1, Rectangle r2)
    {
        return (r1.Left >= r2.Left) && (r1.Left <= r2.Right);
    }

    public static bool IsInsideRight(Rectangle r1, Rectangle r2)
    {
        return (r1.Right >= r2.Left) && (r1.Right <= r2.Right);
    }

    public static bool IsInsideTop(Rectangle r1, Rectangle r2)
    {
        return (r1.Top <= r2.Bottom) && (r1.Top >= r2.Top);
    }

    public static bool IsInsideBottom(Rectangle r1, Rectangle r2)
    {
        return (r1.Bottom <= r2.Bottom) && (r1.Bottom >= r2.Top);
    }

    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        if (IsInsideLeft(r1, r2) || IsInsideLeft(r2, r1) || IsInsideRight(r1, r2) || IsInsideRight(r2, r1))
        {
            if (IsInsideBottom(r1, r2) || IsInsideBottom(r2, r1))
                return true;
            if (IsInsideTop(r1, r2) || IsInsideTop(r2, r1))
                return true;
        }

        return false;
    }

    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        int searchX = 0;
        int searchY = 0;

        if (IndexOfInnerRectangle(r1, r2) > -1)
            SearchAreaInner(r1, r2);

        if (AreIntersected(r1, r2))
        {
            searchX = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left);
            searchY = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top);

            if ((searchX * searchY) < 0)
                searchX *= -1;

            return searchX * searchY;
        }

        return 0;
    }

    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        if (IsInsideBottom(r1, r2) && IsInsideLeft(r1, r2) && IsInsideRight(r1, r2) && IsInsideTop(r1, r2))
            return 0;
        else if (IsInsideBottom(r2, r1) && IsInsideLeft(r2, r1) && IsInsideRight(r2, r1) && IsInsideTop(r2, r1))
            return 1;
        return -1;
    }

    public static int SearchAreaInner(Rectangle r1, Rectangle r2)
    {
        if (IndexOfInnerRectangle(r1, r2) == 0)
            return r1.Height * r1.Width;
        return r2.Height * r2.Width;
    }
}
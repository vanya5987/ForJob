using System;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    class Drawler
    {
        static float X, Y;
        static IGraphics Graphics;

        public static void Initialize(IGraphics graphics)
        {
            Graphics = graphics;
            Graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static void DrawLine(Pen pen, double length, double angle)
        {
            var x = (float)(X + length * Math.Cos(angle));
            var y = (float)(Y + length * Math.Sin(angle));

            Graphics.DrawLine(pen, X, Y, x, y);

            X = x;
            Y = y;
        }

        public static void Change(double length, double angle)
        {
            X = (float)(X + length * Math.Cos(angle));
            Y = (float)(Y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void DrawFigure(int weight, int height, double rotateAngle, IGraphics graphics)
        {
            Drawler.Initialize(graphics);

            var size = Math.Min(weight, height);

            var diagonalLength = Math.Sqrt(2) * (size * 0.375f + size * 0.04f) / 2;
            var x = (float)(diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + weight / 2f;
            var y = (float)(diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            Drawler.SetPosition(x, y);

            DrawSegment(size, 0, Math.PI / 4, Math.PI, Math.PI / 2, -Math.PI, 3 * Math.PI / 4);
            DrawSegment(size, -Math.PI / 2, -Math.PI / 2 + Math.PI / 4, -Math.PI / 2 + Math.PI,
                -Math.PI / 2 + Math.PI / 2, -Math.PI / 2 - Math.PI, -Math.PI / 2 + 3 * Math.PI / 4);
            DrawSegment(size, Math.PI, Math.PI + Math.PI / 4, Math.PI + Math.PI, Math.PI + Math.PI / 2,
                Math.PI - Math.PI, Math.PI + 3 * Math.PI / 4);
            DrawSegment(size, Math.PI / 2, Math.PI / 2 + Math.PI / 4, Math.PI / 2 + Math.PI,
                Math.PI / 2 + Math.PI / 2, Math.PI / 2 - Math.PI, Math.PI / 2 + 3 * Math.PI / 4);
        }

        public static void DrawSegment(int size, double firstConst, double secondConst,
            double thirdConst, double fourConst, double fiveConst, double sixConst)
        {
            Drawler.DrawLine(new Pen(Brushes.Yellow), size * 0.375f, firstConst);
            Drawler.DrawLine(new Pen(Brushes.Yellow), size * 0.04f * Math.Sqrt(2), secondConst);
            Drawler.DrawLine(new Pen(Brushes.Yellow), size * 0.375f, thirdConst);
            Drawler.DrawLine(new Pen(Brushes.Yellow), size * 0.375f - size * 0.04f, fourConst);

            Drawler.Change(size * 0.04f, fiveConst);
            Drawler.Change(size * 0.04f * Math.Sqrt(2), sixConst);
        }
    }
}
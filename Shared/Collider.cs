using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class Collider
    {
        public double Size { get; set; }

        public abstract bool Overlaps(Vector2d position, Collider otherCollider, Vector2d otherPosition);

        public static bool CircleOverlapsSquare(Vector2d circlePos, double circleRadius, Vector2d squarePos, double squareWidth)
        {
            // Check if center of circle is inside square
            if (Math.Abs(circlePos.x - squarePos.x) < squareWidth / 2 && Math.Abs(circlePos.y - squarePos.y) < squareWidth / 2)
                return true;

            // Otherwise, check against all 4 edges of square
            return !(circlePos.x + circleRadius < squarePos.x - squareWidth / 2 ||
                circlePos.x - circleRadius > squarePos.x + squareWidth / 2 ||
                circlePos.y + circleRadius < squarePos.y - squareWidth / 2 ||
                circlePos.y - circleRadius > squarePos.y + squareWidth / 2);
        }

        public static bool CircleOverlapsCircle(Vector2d c1Pos, double c1Radius, Vector2d c2Pos, double c2Radius)
        {
            return Math.Pow(c1Pos.x - c2Pos.x, 2) + Math.Pow(c1Pos.y - c2Pos.y, 2) < Math.Pow(c1Radius + c2Radius, 2);
        }

        public static bool SquareOverlapsSquare(Vector2d s1Pos, double s1Width, Vector2d s2Pos, double s2Width)
        {
            return !(
                s1Pos.x + s1Width / 2 < s2Pos.x - s2Width / 2 ||
                s1Pos.x - s1Width / 2 > s2Pos.x + s2Width / 2 ||
                s1Pos.y + s1Width / 2 < s2Pos.y - s2Width / 2 ||
                s1Pos.y - s1Width / 2 > s2Pos.y + s2Width / 2
                );
        }
    }
}

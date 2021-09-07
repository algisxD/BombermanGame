using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class BoxCollider : Collider
    {
        public BoxCollider(double width)
        {
            Size = width;
        }

        public override bool Overlaps(Vector2d position, Collider otherCollider, Vector2d otherPosition)
        {
            if (otherCollider.GetType() == typeof(BoxCollider))
            {
                return SquareOverlapsSquare(position, Size, otherPosition, otherCollider.Size);
            }

            if (otherCollider.GetType() == typeof(CircleCollider))
            {
                return CircleOverlapsSquare(otherPosition, otherCollider.Size, position, Size);
            }

            throw new NotImplementedException();
        }
    }
}

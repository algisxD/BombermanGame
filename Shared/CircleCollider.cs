using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Bomberman.Shared
{
    public class CircleCollider : Collider
    {
        public CircleCollider(double radius)
        {
            Size = radius;
        }

        public override bool Overlaps(Vector2d position, Collider otherCollider, Vector2d otherPosition)
        {
            if (otherCollider.GetType() == typeof(BoxCollider))
            {
                return CircleOverlapsSquare(position, Size, otherPosition, otherCollider.Size);
            }

            if (otherCollider.GetType() == typeof(CircleCollider))
            {
                return CircleOverlapsCircle(position, Size, otherPosition, otherCollider.Size);
            }

            throw new NotImplementedException();
        }
    }
}

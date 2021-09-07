using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.Objects
{
    [Serializable]
    public class Wall : GameObject
    {
        public override (int x, int y) SpriteIndex => (4, 0);
    }
}

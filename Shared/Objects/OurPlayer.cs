using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Bomberman.Shared.Objects
{
    [Serializable]
    public class OurPlayer : Player
    {
        public override (int x, int y) SpriteIndex => (0, 0);
    }
}

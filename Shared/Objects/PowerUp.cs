using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.Objects
{
    public class PowerUp : GameObject
    {
        public override int GetRenderOrder()
        {
            return 5;
        }
    }
}

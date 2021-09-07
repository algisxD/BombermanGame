using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class CPlayer : GameObject
    {
        public abstract List<string> GetPowerUp();
    }
}

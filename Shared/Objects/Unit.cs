using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class Unit : GameObject
    {
        public bool isShielded = false;
        public bool hasBoots = false;

        public abstract void GetPowerUp();
    }
}

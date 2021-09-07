using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class BombBuildStrategy
    {
        public abstract void BuildExplosion(List<GameObject> Explosion, Bomb Bomb);
    }
}

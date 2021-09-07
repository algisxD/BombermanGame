using System;
using System.Drawing;

namespace Bomberman.Shared
{
    [Serializable]
    public class Bomb : GameObject
    {
        private const int DEFAULT_DETONATION_DELAY = 2000;
        public int bombRadius = 3;

        public int DetonationTime;

        public Bomb()
        {
            this.type = Type.Bomb;
            this.InstanceID = INSTANCE_ID_COUNTER+10000;
            INSTANCE_ID_COUNTER++;
        }

        public override (int x, int y) SpriteIndex => (2, 0);

        public override int GetRenderOrder()
        {
            return 1;
        }
    }
}

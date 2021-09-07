using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Bomberman.Shared
{
    class Fire : GameObject
    {
        public int Duration { get; set; }
        public long BombId { get; set; }

        public Fire(long bombId)
        {
            BombId = bombId;
            this.type = Type.Fire;
            this.InstanceID = INSTANCE_ID_COUNTER+5000;
            INSTANCE_ID_COUNTER++;
        }

        public override (int x, int y) SpriteIndex => (3, 0);

        public override int GetRenderOrder()
        {
            return 3;
        }
    }
}

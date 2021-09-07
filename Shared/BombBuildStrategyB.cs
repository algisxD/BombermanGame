using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class BombBuildStrategyB : BombBuildStrategy
    {
        public override void BuildExplosion(List<GameObject> Explosion, Bomb Bomb)
        {
            for (double i = 1; i < Bomb.bombRadius * World.TileSize; i += World.TileSize)
            {
                CreateFire(Bomb.Position.x, Bomb.Position.y + i, Explosion, Bomb.InstanceID);
            }
            for (double i = 1; i < Bomb.bombRadius * World.TileSize; i += World.TileSize)
            {
                CreateFire(Bomb.Position.x, Bomb.Position.y - i, Explosion, Bomb.InstanceID);
            }
        }

        private void CreateFire(double x, double y, List<GameObject> Explosion, long ParentId)
        {
            Fire fire = new Fire(ParentId);
            fire.Position = new Vector2d(x, y);
            Explosion.Add(fire);
        }
    }
}

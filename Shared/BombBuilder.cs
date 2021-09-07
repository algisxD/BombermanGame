using Bomberman.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bomberman.Shared
{
    public class BombBuilder : AbstractBombBuilder
    {
        private BombBuildStrategy _strategy;
        private GameState _state;
        public override void BuildBomb(Vector2d position, BombBuildStrategy strategy, GameState state)
        {
            _strategy = strategy;
            _state = state;
            base.Bomb = new Bomb();
            base.Bomb.bombRadius = 3;
            base.Bomb.Position = position;
        }

        public override void ExplosionHA()
        {
            Explosion = new List<GameObject>();
            _strategy.BuildExplosion(Explosion, Bomb);
        }

        public override List<GameObject> GetResult()
        {
            var results = new List<GameObject>(Explosion);
            results = CheckBombCollision(this._state, results);
            results.Add(Bomb);
            return results;
        }

        private List<GameObject> CheckBombCollision(GameState state, List<GameObject> builtBomb)
        {
            List<GameObject> collisions = new List<GameObject>();
            foreach(var fire in builtBomb)
            {
                var value = state.gameObjects.FirstOrDefault(x => x.Value.Position.Equals(fire.Position)).Value;
                if (value != null && value.type == GameObject.Type.Wall || value != null && value.type == GameObject.Type.Bomb)
                {
                    collisions.Add(fire);
                }
            }

            builtBomb = builtBomb.Except(collisions).ToList();

            return builtBomb;
        }
    }
}

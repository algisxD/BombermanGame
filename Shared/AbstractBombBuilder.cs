using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class AbstractBombBuilder
    {
        public Bomb Bomb { get; set; }
        public List<GameObject> Explosion { get; set; }
        public abstract void BuildBomb(Vector2d position, BombBuildStrategy strategy, GameState state);
        public abstract void ExplosionHA();
        public abstract List<GameObject> GetResult();

        public void TemplateMethod(Vector2d bombPosition, BombBuildStrategy strategy, GameState state)
        {
            BuildBomb(bombPosition, strategy, state);
            ExplosionHA();
        }
    }
}

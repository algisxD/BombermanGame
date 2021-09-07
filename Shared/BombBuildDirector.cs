using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class BombBuildDirector
    {
        public void Construct(AbstractBombBuilder bombBuilder, Vector2d bombPosition, BombBuildStrategy strategy, GameState state)
        {
            //bombBuilder.BuildBomb(bombPosition, strategy, state);
            //bombBuilder.ExplosionHA();
            bombBuilder.TemplateMethod(bombPosition, strategy, state);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Bomberman.Shared
{
    public class BombermanFacade
    {
        BombBuildDirector director;
        BombBuilder bombBuilder;
        Vector2d moveAdd;

        public BombermanFacade()
        {
            director = new BombBuildDirector();
            bombBuilder = new BombBuilder();
            moveAdd = new Vector2d();
        }

        public void addMove(double x, double y)
        {
            moveAdd.Add(new Vector2d(x, y));
        }
        
        public void BuildBomb(Vector2d bombPosition, BombBuildStrategy _strategy, GameState state)
        {
            director.Construct(bombBuilder, bombPosition, _strategy, state);
        }

        public List<GameObject> getResult()
        {
            List<GameObject> results = bombBuilder.GetResult();
            return results;
        }
    }
}

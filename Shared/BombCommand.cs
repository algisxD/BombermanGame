using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class BombCommand : ICommand
    {
        private Objects.OurPlayer _player;
        private BombermanFacade _facade;
        private GameState _state;
        private GameStateProxy gameState;
        private BombBuildStrategy _strategy;

        public BombCommand(Objects.OurPlayer player, GameState state, BombBuildStrategy strategy)
        {
            _strategy = strategy;
            _player = player;
            _facade = new BombermanFacade();
            _state = state;
            gameState = new GameStateProxy();
        }

        public void Execute()
        {
            Vector2d bombPosition = new Vector2d(Math.Round(_player.Position.x), Math.Round(_player.Position.y));
            _facade.BuildBomb(bombPosition, _strategy, _state);
            var objects = _facade.getResult();

            foreach (GameObject g in objects)
            {
                Console.Write(g.InstanceID);
                Console.Write(g.UserID);
                Console.Write(g.type);
                Console.Write(g.Size);
                g.UserID = _player.UserID;
                _state.Apply(g);
            }
        }

        public void Undo()
        {
        }
}
}

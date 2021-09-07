using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class GameStateProxy : IGameState
    {
        private GameState _gameState { get; set; }
        public Dictionary<long, GameObject> gameObjects = new Dictionary<long, GameObject>();
        public GameStateProxy()
        {
            _gameState = new GameState();
        }
        public void AddPlayer(GameObject player, GameState state)
        {
            if (!player.Deleted && player != null && state != null)
                _gameState.AddPlayer(player, state);
        }

        public void Apply(GameObject gameObject)
        {
            if (!gameObject.Deleted && gameObject != null)
                _gameState.Apply(gameObject);
        }
    }
}

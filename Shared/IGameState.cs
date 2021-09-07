using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public interface IGameState
    {
        public void Apply(GameObject gameObject);
        public void AddPlayer(GameObject player, GameState state);
    }
}

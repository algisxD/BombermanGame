using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.State_DP
{
    public abstract class PlayerState
    {
        protected Player _player;

        public Player player
        {
            get { return _player; }
            set { _player = value; }
        }
        public abstract void HandleState(Player player);
    }
}

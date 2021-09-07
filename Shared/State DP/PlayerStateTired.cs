using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.State_DP
{
    class PlayerStateTired : PlayerState
    {
        public PlayerStateTired(PlayerState playerState) : this(playerState.player)
        {
        }
        public PlayerStateTired(Player player)
        {
            this.player = player;
            Initialize();
        }

        private void Initialize()
        {
            _player.Lives = 1;
            _player.MovementSpeed = 0.7;
        }

        public override void HandleState(Player player)
        {
            player.playerState = new PlayerStateNormal(this);
        }
    }
}

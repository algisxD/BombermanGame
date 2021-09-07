using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.State_DP
{
    class PlayerStateEnraged : PlayerState
    {
        public PlayerStateEnraged(PlayerState playerState) : this(playerState.player)
        {
        }
        public PlayerStateEnraged(Player player)
        {
            this.player = player;
            Initialize();
        }
        private void Initialize()
        {
            _player.Lives = 3;
            _player.MovementSpeed = 1.3;
        }

        public override void HandleState(Player player)
        {
            player.playerState = new PlayerStateTired(this);
        }
    }
}

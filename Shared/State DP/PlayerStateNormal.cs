using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared.State_DP
{
    public class PlayerStateNormal : PlayerState
    {
        public PlayerStateNormal(PlayerState playerState) : this(playerState.player)
        {
        }
        public PlayerStateNormal(Player player)
        {
            this.player = player;
            Initialize();
        }

        private void Initialize()
        {
            _player.Lives = 1;
            _player.MovementSpeed = 1;
        }

        public override void HandleState(Player player)
        {
            player.playerState = new PlayerStateEnraged(this);
        }
    }
}

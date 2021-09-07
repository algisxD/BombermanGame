using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Collections;
using Bomberman.Shared.State_DP;
using Newtonsoft.Json;

namespace Bomberman.Shared

{
    [Serializable]
    public partial class Player : CPlayer
    {
        public string Name = "Player";
        public bool isHost = false;
        private bool isDead = false;
        private double movementSpeed = 1;
        private int lives = 1;
        private List<string> powerUps = new List<string>();
        private PlayerState _state;

        public MovementDirection Orientation  = MovementDirection.NONE;

        public Player()
        {
            _state = new PlayerStateNormal(this);
        }

        [JsonIgnore]
        public PlayerState playerState
        {
            get { return _state; }
            set { _state = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        
        public double MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }
        }

        public enum MovementDirection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NONE
        }

        public override (int x, int y) SpriteIndex => (1, 0);

        public override int GetRenderOrder()
        {
            return 10;
        }

        public override List<string> GetPowerUp()
        {
            return powerUps;
        }
        
        public void setFirstState()
        {
            _state = new PlayerStateNormal(this);
        }

        public void ChangeState()
        {
            _state.HandleState(this);
        }
    }
}

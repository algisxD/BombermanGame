using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class ShieldDecorator : Decorator
    {
        public ShieldDecorator(CPlayer player) : base(player){ }

        public override List<string> GetPowerUp()
        {
            List<string> powerUp = base.GetPowerUp();
            powerUp.Add("Shield");
            return powerUp;
        }
    }
}

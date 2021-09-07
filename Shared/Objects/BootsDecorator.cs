using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bomberman.Shared
{
    public class BootsDecorator : Decorator
    {
        public BootsDecorator(CPlayer player) : base(player) { }

        public override List<string> GetPowerUp()
        {
            List<string> powerUp = base.GetPowerUp();
            powerUp.Add("Boots");
            return powerUp;
        }
    }
}

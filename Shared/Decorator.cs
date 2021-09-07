using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public abstract class Decorator : CPlayer
    {
        protected CPlayer _component;

        public Decorator(CPlayer component)
        {
            _component = component;
        }

        public override List<string> GetPowerUp()
        {
            return _component.GetPowerUp();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public interface IGameObjectFactory
    {
        public GameObject Create(NetworkObject from);
    }
}

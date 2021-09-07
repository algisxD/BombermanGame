using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public interface ISubject
    {
        void Attach(GameObject observer);
        void Detach(GameObject observer);
        void Notify(GameState state);
    }
}

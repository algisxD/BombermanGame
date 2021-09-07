using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}

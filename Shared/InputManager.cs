using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class InputManager
    {
        private List<ICommand> _commandList = new List<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            _commandList.Add(command);
            command.Execute();
        }

        public void UndoCommands()
        {
            int lastElement = _commandList.Count - 1;
            _commandList[lastElement].Undo();
            _commandList.RemoveAt(lastElement);
        }
    }
}

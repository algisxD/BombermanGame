using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class MoveCommand : ICommand
    {
        private Vector2d _direction;
        private Objects.OurPlayer _player;

        public MoveCommand(Objects.OurPlayer player, Vector2d direction)
        {
            _direction = direction;
            _player = player;
        }

        public void Execute()
        {
            _player.Position =_player.Position.Add(_direction);
        }

        public void Undo()
        {
            _player.Position = _player.Position.Substract(_direction);
        }
    }
}

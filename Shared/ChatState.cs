using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    /// <summary>
    /// The 'ConcreteMediator' class
    /// </summary>

    public class Chatroom : AbstractChatroom
    {
        private Dictionary<string, Player> _participants = new Dictionary<string, Player>();
        public Queue<string> Chat = new Queue<string>();

        public override void Register(Player participant)
        {
            if (!_participants.ContainsValue(participant))
            {
                _participants[participant.Name] = participant;
            }

            participant.Chatroom = this;
        }

        public override void Apply(string from, string message)
        {
            Chat.Enqueue(from + ": " + message);
        }
    }
}

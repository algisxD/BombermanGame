using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    /// <summary>
    /// The 'Mediator' abstract public class
    /// </summary>
    abstract public class AbstractChatroom
    {
        public abstract void Register(Player participant);
        public abstract void Apply(string from, string message);
    }

    /// <summary>
    /// The 'AbstractColleague' public class
    /// </summary>
    public partial class Player
    {
        // Constructor
        public Player(string name)
        {
            this.Name = name;
        }

        public Chatroom Chatroom { set; get; }

        // Sends message to given participant
        public void Send(string message)
        {
            Chatroom.Apply(Name, message);
        }

        // Receives message from given participant
        public virtual void Receive(string from, string message)
        {
            Console.WriteLine("{0} to {1}: '{2}'",
              from, Name, message);
        }
    }

    /// <summary>
    /// A 'ConcreteColleague' public class
    /// </summary>
    public class Teammate : Player
    {
        // Constructor
        public Teammate(string name)
          : base(name)
        {
        }

        public override void Receive(string from, string message)
        {
            Console.Write("To a team mate: ");
            base.Receive(from, message);
        }
    }

    /// <summary>
    /// A 'ConcreteColleague' public class
    /// </summary>
    public class Enemy : Player
    {
        // Constructor
        public Enemy(string name)
          : base(name)
        {
        }

        public override void Receive(string from, string message)
        {
            Console.Write("To a enemy: ");
            base.Receive(from, message);
        }
    }
}
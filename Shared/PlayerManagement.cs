using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class PlayerManagement : ISubject
    {
        private List<GameObject> _players = new List<GameObject>();
        public int PlayersToStart;
        public int JoinedPlayers;
        private GameStateProxy _gameState { set; get; }

        public PlayerManagement()
        {
            PlayersToStart = 1;
            JoinedPlayers = 0;
        }

        public void Attach(GameObject player)
        {
            JoinedPlayers++;
            this._players.Add(player);
        }

        public void Detach(GameObject player)
        {
            JoinedPlayers--;
            this._players.Remove(player);
        }

        public void Notify(GameState state)
        {
            foreach (var player in _players)
            {
                Console.WriteLine(player.InstanceID);
                Console.WriteLine(player.controlsEnabled);
                player.Update();
                Console.WriteLine(player.controlsEnabled);
            }
        }

        public void CheckIfReady(GameState state)
        {
            if (PlayersToStart <= JoinedPlayers)
                this.Notify(state);
        }

        public void Print()
        {
            foreach(var ob in _players)
            {
                Console.WriteLine(ob.InstanceID);
                //Console.WriteLine(_gamestate);
            }
        }
    }
}

using Bomberman.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace Bomberman.Shared
{
    [Serializable]
    public class GameState : IGameState
    {
        // GameState consists of list of GameObjects that can be modified, created, deleted
        // Indexed by their instance id
        public Dictionary<long, GameObject> gameObjects = new Dictionary<long, GameObject>();
        private PlayerManagement Players = new PlayerManagement();


        public void Apply(GameObject gameObject)
        {
            // Check if GameObject was deleted
            if (gameObject.Deleted)
            {
                if (gameObjects.ContainsKey(gameObject.InstanceID))
                {
                    gameObjects.Remove(gameObject.InstanceID);
                }
            }

            // Otherwise, check if this is a new GameObject that needs an ID assigned (SERVER ONLY)
            if (gameObject.InstanceID == 0)
            {
                gameObject.InstanceID = GameObject.INSTANCE_ID_COUNTER++;
                gameObjects.Add(gameObject.InstanceID, gameObject);
            }

            // Otherwise, this is a modification to an object
            if (gameObjects.ContainsKey(gameObject.InstanceID))
            {
                gameObjects[gameObject.InstanceID] = gameObject;
            }
            else
            {
                gameObjects.Add(gameObject.InstanceID, gameObject);
            }
        }

        public void AddPlayer(GameObject player, GameState state)
        {
            Players.Attach(player);
            Players.CheckIfReady(state);
        }
    }
}

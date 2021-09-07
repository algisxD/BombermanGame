using Bomberman.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Bomberman.Server.Hubs
{
    public class GameObjectHub : Hub
    {
        private OurGameObjectFactory factory = new OurGameObjectFactory();

        public async Task SendNetworkObjects(string gameObjectData)
        {
            NetworkObject[] netObjects = JsonConvert.DeserializeObject<NetworkObject[]>(gameObjectData);
            foreach (NetworkObject netObject in netObjects)
            {
                // Add gameobject to gamestate
                Startup.GameState.Apply(factory.Create(netObject));
            }

            NetworkObject[] toSend = Startup.GameState.gameObjects.Values.ToList()
                .Where(g => g.type != GameObject.Type.Ground && g.type != GameObject.Type.Wall) // dont sync ground and walls
                .Select(g => new NetworkObject
            {
                Type = g.type,
                InstanceID = g.InstanceID,
                OwnerID = g.UserID,
                Data = JsonConvert.SerializeObject(g)
            }).ToArray();
           

            await Clients.All.SendAsync("ReceiveNetworkObjects", JsonConvert.SerializeObject(toSend));
        }

        public async Task SendChatMessage(string from, string message)
        {
            Startup.Chatroom.Apply(from, message);
            await Clients.All.SendAsync("ReceiveChatMessage", from, message);
        }
    }
}

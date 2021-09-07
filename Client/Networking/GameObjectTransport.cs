using Bomberman.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Bomberman.Shared.Objects;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Collections.Generic;

namespace Bomberman.Client.Networking
{
    public class GameObjectTransport : IDisposable
    {
        public static GameObjectTransport Instance { get; } = new GameObjectTransport();
        public event Action OnReceivedMessage;
        private HubConnection connection;
        private HttpClient http;

        // The Game State that is controlled by this transport
        public GameState GameState { get; private set; }
        public Chatroom Chatroom { get; private set; }

        // The Unique User Id for us in this game session
        public OurPlayer PlayerObject { get; private set; }
        public bool IsConnectionInitialized { get; private set; }

        private IGameObjectFactory ourFactory = new OurGameObjectFactory();
        private IGameObjectFactory theirFactory = new TheirGameObjectFactory();

        private GameObjectTransport() { }

        public void Initialize(NavigationManager navigationManager, HttpClient http)
        {
            if (IsConnectionInitialized)
                return;
            connection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/gameobjecthub"))
                .AddNewtonsoftJsonProtocol()
                .Build();

            this.http = http;
            IsConnectionInitialized = true;
        }

        public async Task Connect()
        {
            // Create a new User Id for us
            HttpResponseMessage idResponse = await http.GetAsync("api/users");
            NetworkObject playerNetObject = JsonConvert.DeserializeObject<NetworkObject>(await idResponse.Content.ReadAsStringAsync());
            PlayerObject = ourFactory.Create(playerNetObject) as OurPlayer;

            Chatroom = new Chatroom();
            GameState = new GameState();
            GameState.Apply(PlayerObject);
            //GameState.AddPlayer(PlayerObject);

            // Fetch the game state from the server
            HttpResponseMessage responseMessage = await http.GetAsync("api/gamestate");
            string rawData = await responseMessage.Content.ReadAsStringAsync();
            foreach (NetworkObject netOb in JsonConvert.DeserializeObject<NetworkObject[]>(rawData))
            {
                ReceivedNetworkObject(netOb);
            }

            connection.On<string>("ReceiveNetworkObjects", (netObData) =>
            {
                foreach (NetworkObject netOb in JsonConvert.DeserializeObject<NetworkObject[]>(netObData))
                {
                    ReceivedNetworkObject(netOb);
                }
            });

            connection.On<string, string>("ReceiveChatMessage", (from, message) =>
            {
                ReceivedChatMessage(from, message);
            });

            await connection.StartAsync();

            // Send out objects that belong to us to the server repeatedly
            Timer t = new Timer(20);
            t.Elapsed += (e, args) =>
            {
                Send(GameState.gameObjects.Values.Where(g => g.UserID == PlayerObject.InstanceID)).ConfigureAwait(false);
            };
            t.Start();
        }

        private void ReceivedNetworkObject(NetworkObject netOb)
        {
            // Don't apply our objects - we will handle them on our end
            if (netOb.OwnerID == PlayerObject.InstanceID) return;

            GameObject go = GetAssociatedFactory(netOb).Create(netOb);
            GameState.Apply(go);
        }

        private void ReceivedChatMessage(string from, string message)
        {
            Chatroom.Apply(from, message);
            OnReceivedMessage?.Invoke();
        }

        private IGameObjectFactory GetAssociatedFactory(NetworkObject netOb)
        {
            if (netOb.OwnerID == PlayerObject.InstanceID) return ourFactory;
            return theirFactory;
        }

        private async Task Send(IEnumerable<GameObject> gameObjects)
        {
            NetworkObject[] toSend = gameObjects
                .Select(g => new NetworkObject
                {
                    Type = g.type,
                    OwnerID = g.UserID,
                    InstanceID = g.InstanceID,
                    Data = JsonConvert.SerializeObject(g)
                }).ToArray();

            await connection.SendAsync("SendNetworkObjects", JsonConvert.SerializeObject(toSend));
        }

        public async Task Send(string message)
        {
            string from = PlayerObject.Name + PlayerObject.UserID;
            await connection.SendAsync("SendChatMessage", from, message);
        }

        public void Dispose()
        {
            _ = connection.DisposeAsync();
        }
    }
}

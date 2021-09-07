using Bomberman.Shared.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    public class OurGameObjectFactory : IGameObjectFactory
    {
        public GameObject Create(NetworkObject from)
        {
            switch (from.Type)
            {
                case GameObject.Type.Player:
                    return JsonConvert.DeserializeObject<OurPlayer>(from.Data).SetCollider(new CircleCollider(0.35f));
                case GameObject.Type.Bomb:
                    return JsonConvert.DeserializeObject<OurBomb>(from.Data);
                case GameObject.Type.Wall:
                    return JsonConvert.DeserializeObject<Wall>(from.Data).SetCollider(new BoxCollider(1f));
                default:
                    return JsonConvert.DeserializeObject<GameObject>(from.Data);
            }
        }
    }
}

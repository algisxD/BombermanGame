using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Shared
{
    [Serializable]
    public class NetworkObject
    {
        public long InstanceID { get; set; }
        public long OwnerID { get; set; }
        public GameObject.Type Type { get; set; }
        public string Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Bomberman.Shared.State_DP;
using Newtonsoft.Json;

namespace Bomberman.Shared
{
    [Serializable]
    public class GameObject : ICloneable, IObserver
    {
        public const double PLAYER_MOVE_SPEED = 3;
        public bool controlsEnabled = false;

        public enum Type
        {
            Wall = 1,
            Ground = 0,
            Player = 2,
            Bomb = 3,
            Fire = 4,
            PowerUp = 5
        }
        // Instance ID is primarily used for networking, NOT database and serialization
        // New GameObjects, that haven't received an ID, will have their ID set to 0
        public static long INSTANCE_ID_COUNTER { get; set; } = 1;
        public long InstanceID { get; set; }

        // True if GameObject was deleted and should be removed and dereferenced
        public bool Deleted { get; set; }

        // Position
        public Vector2d Position { get; set; }

        public Type type;

        // Size
        public double Size { get; set; } = 1;
        public long UserID { get; set; }

        // The collider of the object
        [JsonIgnore]
        public Collider Collider { get; set; }

        /// <summary>
        /// The index of the sprite in the sprite sheet, where 0;0 is upper-left
        /// </summary>
        public virtual (int x, int y) SpriteIndex => (7, 7);

        public bool PhysicsOverlapsWith(GameObject other)
        {
            return PhysicsOverlapsWith(other.Collider, other.Position);
        }

        public GameObject SetCollider (Collider col)
        {
            Collider = col;
            return this;
        }

        public bool PhysicsOverlapsWith(Collider collider, Vector2d position)
        {
            if (Collider == null || collider == null) return false;
            return Collider.Overlaps(Position, collider, position);
        }

        public virtual int GetRenderOrder()
        {
            return 0;
        }

        public object Clone()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GameObject>(serialized);
        }

        public void Update()
        {
            controlsEnabled = true;
        }
    }
}

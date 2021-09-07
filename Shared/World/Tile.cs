using System;

namespace Bomberman.Shared
{
    [Serializable]
    public class Tile 
    {
        public const int SIZE = 1;
        public bool Walkable = false;
        public bool Destroyable = false;
        public bool Occupied = false;
        public bool Fire = false;

        [NonSerialized]
        public int bonusHere = 0;
        [NonSerialized]
        public int bomb = 0;

        public Tile(bool walkable, bool destroyable)
        {
            Walkable = walkable;
            Destroyable = destroyable;
        }


        public void SpawnBonus()
        {

        }

        public void Draw()
        {
            
        }
    }
}

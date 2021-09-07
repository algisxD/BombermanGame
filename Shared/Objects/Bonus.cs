
using System;
using Newtonsoft.Json;

namespace Bomberman.Shared

{
    public class Bonus : GameObject
    {
        public BonusType Type { get; set; } = BonusType.None;
        public Tile Tile { get; private set; }

        public Bonus(Tile tile, BonusType type)
        {
            this.Type = type;
            this.Tile = tile;
        }

		public GameObject makeCopy()
        {
            GameObject sheepObject = (GameObject)base.Clone();

            return sheepObject;
        }
	}

    public enum BonusType
    {
        None,
        PowerBomb,
        SpeedBoost,
        Armor
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Bomberman.Shared
{
    [Serializable]
    public class World 
    {
        public const int RES_X = 640;
        public const int RES_Y = 640;
        public int[,] MapGrid;


        public static int TilesCountInOneDimention { get; set; } = 10;
        public static double TileSize => RES_X / TilesCountInOneDimention / 64d;
    }
}

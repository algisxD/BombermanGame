using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace Bomberman.Shared
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Vector2d
    {
        public double x;
        public double y;

        public Vector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        
    }
    public static class ExtensionMethods
    {
        public static T CloneWithBinaryFormatter<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static  Vector2d WithAddedY(this Vector2d vector, double y)
        {
            return new Vector2d(vector.x, vector.y + y);
        }

        public static Vector2d WithAddedX(this Vector2d vector, double x)
        {
            return new Vector2d(vector.x + x, vector.y);
        }

        public static Vector2d Add (this Vector2d vector, Vector2d toAdd)
        {
            return new Vector2d(vector.x + toAdd.x, vector.y + toAdd.y);
        }

        public static Vector2d Substract(this Vector2d vector, Vector2d toAdd)
        {
            return new Vector2d(vector.x - toAdd.x, vector.y - toAdd.y);
        }

        public static bool IsZero(this Vector2d vector)
        {
            return vector.x == 0 && vector.y == 0;
        }
    }
}


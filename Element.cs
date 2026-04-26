using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class Element(string name, int X, int Y, Texture2D texture)
    {
        public string Name { get; set; } = name;
        public Texture2D Texture { get; set; } = texture;
        public double WorldX { get; set; } = X;
        public double WorldY { get; set; } = Y;
    }
}
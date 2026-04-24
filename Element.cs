using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class Element(string name, int X, int Y, string texture)
    {
        public string Name { get; set; } = name;
        public Texture2D Texture { get; set; } = OpenSource.OpenPng(texture);
        public int WorldX { get; set; } = X;
        public int WorldY { get; set; } = Y;
    }
}
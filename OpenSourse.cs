using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class OpenSource
    {
        static public GraphicsDevice GraphicsDevice { get; set; }
        static public Texture2D OpenPng(string file_path)
        {
            using var stream = System.IO.File.OpenRead(file_path);
            return Texture2D.FromStream(GraphicsDevice, stream);
        }
    }
}

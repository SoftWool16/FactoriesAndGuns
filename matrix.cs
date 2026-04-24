using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class Matrix
    {
        static public double WorldPosX { get; set; } = 0;
        static public double WorldPosY { get; set; } = 0;
        static public double ScreenPosX { get; set; }
        static public double ScreenPosY { get; set; }
        static public double SizeX { get; set; } = 60;
        static public double SizeY { get; set; } = 30;
        static public SpriteBatch SpriteBatch { get; set; }
        static public GameWindow GameWindow { get; set; }
        static public Field Field { get; set; }
        static public double StaticSize { get; set; } = 50;

        static public void RenderMatrix()
        {
            StaticSize = GameWindow.ClientBounds.Width / SizeX;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    if (
                        j + (int)WorldPosY >= 0 &&
                        j + (int)WorldPosY < Field.SizeY &&
                        i + (int)WorldPosX >= 0 &&
                        i + (int)WorldPosX < Field.SizeX
                    )
                    {
                        double nX = WorldPosX * StaticSize % StaticSize;
                        double nY = WorldPosY * StaticSize % StaticSize;
                        Rectangle destinationRectangle = new(
                            i * (int)StaticSize - (int)nX, // Координата XЯ
                            j * (int)StaticSize - (int)nY, // Координата Y
                            (int)StaticSize,  // Ширина в пикселях
                            (int)StaticSize   // Высота в пикселях
                        );
                        SpriteBatch.Draw(
                            Field.Fields[j + (int)WorldPosY, i + (int)WorldPosX].Texture,
                            destinationRectangle,
                            Color.Gray
                        );
                    }
                }
            }
        }
    }
}

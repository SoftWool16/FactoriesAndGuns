using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class MatrixCamera
    {
        static public double WorldPosX { get; set; } = 0; // Координаты камеры
        static public double WorldPosY { get; set; } = 0;
        static public double SizeX { get; set; } = 60; // Кол-во блоков, которых нужно отрисовать по X
        static public double SizeY { get; set; } = 30; // Кол-во блоков, которых нужно отрисовать по Y
        static public SpriteBatch SpriteBatch { get; set; }
        static public GameWindow GameWindow { get; set; }
        static public Field Field { get; set; }
        static public double StaticSize { get; set; } = 50; // Масштаб

        static public void RenderMatrix()
        {
            StaticSize = GameWindow.ClientBounds.Width / SizeX; // Установка масштаба объектов с учётом кол-ва блоков, которых нужно отрисовать по X ( ширина_окна_в_пикселях / кол-во_блоков.по_X )
            //int n = GameWindow.ClientBounds.Width / GameWindow.ClientBounds.Height;

            double X2 = SizeX / 2;
            double Y2 = SizeY / 2;

            for (int i = -(int)X2; i < X2; i++)
            {
                for (int j = -(int)Y2; j < Y2; j++)
                {
                    if (
                        j + (int)WorldPosY >= 0 &&
                        j + (int)WorldPosY < Field.SizeY &&
                        i + (int)WorldPosX >= 0 &&
                        i + (int)WorldPosX < Field.SizeX
                    )
                    {
                        double nX = WorldPosX * StaticSize % StaticSize; // Получение дробного числа ( дробные.координаты * размер.объекта % размер.объекта )
                        double nY = WorldPosY * StaticSize % StaticSize;
                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            (int)((i + X2) * StaticSize - nX), // Координата X ( индекс * (int)размер.объекта - (int)дробное.число )
                            (int)((j + Y2) * StaticSize - nY), // Координата Y
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

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
            SizeY = GameWindow.ClientBounds.Height / StaticSize;

            double X2 = SizeX / 2;
            double Y2 = SizeY / 2;

            for (int i = -(int)X2; i < X2; i++) // Отрисовка фона
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
                            (int)((i + X2) * StaticSize - nX), // Координата X ( (int)((индекс.поX + половина.масштабаX) * размер.объекта - дробное.число) )
                            (int)((j + Y2) * StaticSize - nY), // Координата Y
                            (int)StaticSize,  // Ширина в пикселях
                            (int)StaticSize   // Высота в пикселях
                        );
                        SpriteBatch.Draw(
                            ContentMaster.Textures[
                                Field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].TexturePath
                                ][
                                Field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].Name
                                ],
                            destinationRectangle,
                            Color.Gray
                        );
                    }
                }
            }

            var list = Field.FieldEquipment.Keys;
            foreach (string name in list) // Отрисовка техники
            {
                var unit = Field.FieldEquipment[name];
                if (
                    unit.WorldX >= WorldPosX - X2 &&
                    unit.WorldX <= WorldPosX + X2 &&
                    unit.WorldY >= WorldPosY - Y2 &&
                    unit.WorldY <= WorldPosY + Y2
                )
                {
                    Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                        (int)((unit.WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                        (int)((unit.WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                        (int)(StaticSize * unit.FixedSizeX),  // Ширина в пикселях
                        (int)(StaticSize * unit.FixedSizeY)   // Высота в пикселях
                    );
                    SpriteBatch.Draw(
                        ContentMaster.Textures[unit.TexturePath]["body"],
                        destinationRectangle,
                        null,
                        Color.Gray,
                        (float)unit.Rotation,
                        new Vector2 (
                            (float)(ContentMaster.Textures[unit.TexturePath]["body"].Width / 2 * unit.OffsetX),
                            (float)(ContentMaster.Textures[unit.TexturePath]["body"].Height / 2 * unit.OffsetY)
                            ),                                                                                       
                        SpriteEffects.None,
                        0
                    );
                    if (unit.Guns != null)
                    {
                        var gunsList = unit.Guns.Keys;
                        foreach (string nameGun in gunsList) // Отрисовка орудий ( если есть )
                        {
                            Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX + unit.Guns[nameGun].OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                            (int)((unit.WorldY + unit.Guns[nameGun].OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                            (int)(StaticSize * unit.FixedSizeX),  // Ширина в пикселях
                            (int)(StaticSize * unit.FixedSizeY * 3.5)   // Высота в пикселях
                            );
                            SpriteBatch.Draw(
                                ContentMaster.Textures[unit.TexturePath][unit.Guns[nameGun].TextureName],
                                destinationRectangleGuns,
                                null,
                                Color.Gray,
                                (float)unit.Guns[nameGun].Rotation,
                                new Vector2(
                                    ContentMaster.Textures[unit.TexturePath][unit.Guns[nameGun].TextureName].Width / 2,
                                    ContentMaster.Textures[unit.TexturePath][unit.Guns[nameGun].TextureName].Height / 2
                                    ),
                                SpriteEffects.None,
                                0
                            );
                        }
                    }
                    if (unit.Effects != null)
                    {
                        var effectList = unit.Effects.Keys;
                        foreach (string nameEffect in effectList) // Отрисовка эффектов ( если есть )
                        {
                            Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX + unit.Effects[nameEffect].OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                            (int)((unit.WorldY + unit.Effects[nameEffect].OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                            (int)(StaticSize * unit.FixedSizeX),  // Ширина в пикселях
                            (int)(StaticSize * unit.FixedSizeY)   // Высота в пикселях
                        );
                            SpriteBatch.Draw(
                                ContentMaster.Textures[unit.TexturePath][unit.Effects[nameEffect].TextureName],
                                destinationRectangleEffects,
                                null,
                                Color.Gray,
                                (float)unit.Effects[nameEffect].Rotation,
                                new Vector2(
                                    ContentMaster.Textures[unit.TexturePath][unit.Effects[nameEffect].TextureName].Width / 2,
                                    ContentMaster.Textures[unit.TexturePath][unit.Effects[nameEffect].TextureName].Height / 2
                                    ),
                                SpriteEffects.None,
                                0
                            );
                        }
                    }
                }
            }
        }
    }
}
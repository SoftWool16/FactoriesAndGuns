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
                if (
                    Field.FieldEquipment[name].WorldX >= WorldPosX - X2 &&
                    Field.FieldEquipment[name].WorldX <= WorldPosX + X2 &&
                    Field.FieldEquipment[name].WorldY >= WorldPosY - Y2 &&
                    Field.FieldEquipment[name].WorldY <= WorldPosY + Y2
                )
                {
                    Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                        (int)((Field.FieldEquipment[name].WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                        (int)((Field.FieldEquipment[name].WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeX),  // Ширина в пикселях
                        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeY)   // Высота в пикселях
                    );
                    SpriteBatch.Draw(
                        ContentMaster.Textures[
                            Field.FieldEquipment[name].TexturePath
                            ]["body"],
                        destinationRectangle,
                        null,
                        Color.Gray,
                        (float)Field.FieldEquipment[name].Rotation,
                        new Vector2 (
                            (float)(ContentMaster.Textures[Field.FieldEquipment[name].TexturePath]["body"].Width / 2 * Field.FieldEquipment[name].OffsetX),
                            (float)(ContentMaster.Textures[Field.FieldEquipment[name].TexturePath]["body"].Height / 2 * Field.FieldEquipment[name].OffsetY)
                            ),
                        SpriteEffects.None,
                        0
                    );
                    //if (Field.FieldEquipment[name].Guns != null)
                    //{
                    //    var gunsList = Field.FieldEquipment[name].Guns.Keys;
                    //    foreach (string nameGun in gunsList) // Отрисовка орудий ( если есть )
                    //    {
                    //        Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                    //        (int)((Field.FieldEquipment[name].WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                    //        (int)((Field.FieldEquipment[name].WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                    //        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeX),  // Ширина в пикселях
                    //        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeY)   // Высота в пикселях
                    //        );
                    //        SpriteBatch.Draw(
                    //            texture: Field.FieldEquipment[name].Guns[nameGun].Texture,
                    //            position: new Vector2(
                    //                (float)((Field.FieldEquipment[name].WorldX - WorldPosX + X2) * StaticSize),
                    //                (float)((Field.FieldEquipment[name].WorldY - WorldPosY + Y2) * StaticSize)
                    //                ),
                    //            sourceRectangle: null,
                    //            color: Color.Gray,
                    //            rotation: (float)Field.FieldEquipment[name].Guns[nameGun].Rotation,
                    //            origin: Field.FieldEquipment[name].CenterPos,
                    //            (float)(StaticSize * Field.FieldEquipment[name].FixedSizeX),
                    //            SpriteEffects.None,
                    //            0f
                    //        );
                    //    }
                    //}
                    //if (Field.FieldEquipment[name].Effects != null)
                    //{
                    //    var effectList = Field.FieldEquipment[name].Effects.Keys;
                    //    foreach (string nameEffect in effectList) // Отрисовка эффектов ( если есть )
                    //    {
                    //        Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                    //        (int)((Field.FieldEquipment[name].WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                    //        (int)((Field.FieldEquipment[name].WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                    //        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeX),  // Ширина в пикселях
                    //        (int)(StaticSize * Field.FieldEquipment[name].FixedSizeY)   // Высота в пикселях
                    //    );
                    //        SpriteBatch.Draw(
                    //            Field.FieldEquipment[name].Effects[nameEffect].Texture,
                    //            destinationRectangleEffects,
                    //            Color.Gray
                    //        );
                    //    }
                    //}
                }
            }
        }
    }
}
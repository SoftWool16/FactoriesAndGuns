using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Factories_And_Guns
{
    internal class MatrixCamera
    {
        static public float WorldPosX { get; set; } = 0; // Координаты камеры
        static public float WorldPosY { get; set; } = 0;
        static public float SizeX { get; set; } = 60; // Кол-во блоков, которых нужно отрисовать по X
        static public float SizeY { get; set; } = 30; // Кол-во блоков, которых нужно отрисовать по Y
        static public SpriteBatch SpriteBatch { get; set; }
        static public GameWindow GameWindow { get; set; }
        static public Field Field { get; set; }
        static public float StaticSize { get; set; } = 50; // Масштаб

        static public void RenderMatrix()
        {
            StaticSize = GameWindow.ClientBounds.Width / SizeX; // Установка масштаба объектов с учётом кол-ва блоков, которых нужно отрисовать по X ( ширина_окна_в_пикселях / кол-во_блоков.по_X )
            SizeY = GameWindow.ClientBounds.Height / StaticSize;

            float X2 = SizeX / 2;
            float Y2 = SizeY / 2;

            for (int i = -(int)X2 - 1; i < X2 + 1; i++) // Отрисовка фона
            {
                for (int j = -(int)Y2 - 1; j < Y2 + 1; j++)
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
                            (int)StaticSize + 1,  // Ширина в пикселях
                            (int)StaticSize + 1   // Высота в пикселях
                        );
                        SpriteBatch.Draw(
                            ContentMaster.Textures[
                                Field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].TexturesFolderPath
                                ][
                                Field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].Name
                                ],
                            destinationRectangle,
                            Color.White
                        );
                    }
                }
            }

            for (int i = -(int)X2; i < X2; i++) // Отрисовка теней построек
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
                        BaseFactory factory = Field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
                        if (factory != null)
                        {
                            float nX = WorldPosX * StaticSize % StaticSize; // Получение дробного числа ( дробные.координаты * размер.объекта % размер.объекта )
                            float nY = WorldPosY * StaticSize % StaticSize;
                            Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                                (int)((i + X2 + (factory.Size / 2)) * StaticSize - nX), // Координата X ( (int)((индекс.поX + половина.масштабаX) * размер.объекта - дробное.число) )
                                (int)((j + Y2 + (factory.Size / 2)) * StaticSize - nY), // Координата Y
                                (int)(StaticSize * factory.Size * 1.15),  // Ширина в пикселях
                                (int)(StaticSize * factory.Size * 1.15)   // Высота в пикселях
                            );
                            Texture2D shadow = ContentMaster.Textures["Block"]["shadow"];
                            SpriteBatch.Draw(
                                shadow,
                                destinationRectangle,
                                null,
                                Color.White,
                                0,
                                new Vector2(
                                    shadow.Width / 2,
                                    shadow.Height / 2
                                    ),
                                SpriteEffects.None,
                                0
                            );
                        }
                    }
                }
            }

            for (int i = -(int)X2; i < X2; i++) // Отрисовка движущихся элементов построек
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
                        BaseFactory factory = Field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
                        if (factory != null)
                        {
                            MovingParts effect = factory.MovingParts;
                            if (effect != null)
                            {
                                float nX = WorldPosX * StaticSize % StaticSize; // Получение дробного числа ( дробные.координаты * размер.объекта % размер.объекта )
                                float nY = WorldPosY * StaticSize % StaticSize;
                                Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                                    (int)((i + X2 + (factory.Size / 2)) * StaticSize - nX), // Координата X ( (int)((индекс.поX + половина.масштабаX) * размер.объекта - дробное.число) )
                                    (int)((j + Y2 + (factory.Size / 2)) * StaticSize - nY), // Координата Y
                                    (int)(StaticSize * effect.Size),  // Ширина в пикселях
                                    (int)(StaticSize * effect.Size)   // Высота в пикселях
                                );
                                Texture2D effect1 = ContentMaster.Textures[factory.TexturesFolderPath]["part0"];
                                SpriteBatch.Draw(
                                    effect1,
                                    destinationRectangle,
                                    null,
                                    Color.White,
                                    effect.Rotation,
                                    new Vector2(
                                        effect1.Width / 2,
                                        effect1.Height / 2
                                        ),
                                    SpriteEffects.None,
                                    0
                                );
                            }
                        }
                    }
                }
            }

            for (int i = -(int)X2; i < X2; i++) // Отрисовка тел построек
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
                        BaseFactory factory = Field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
                        if (factory != null)
                        {
                            float nX = WorldPosX * StaticSize % StaticSize; // Получение дробного числа ( дробные.координаты * размер.объекта % размер.объекта )
                            float nY = WorldPosY * StaticSize % StaticSize;
                            Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                                (int)((i + X2) * StaticSize - nX), // Координата X ( (int)((индекс.поX + половина.масштабаX) * размер.объекта - дробное.число) )
                                (int)((j + Y2) * StaticSize - nY), // Координата Y
                                (int)(StaticSize * factory.Size),  // Ширина в пикселях
                                (int)(StaticSize * factory.Size)   // Высота в пикселях
                            );
                            Texture2D body = ContentMaster.Textures[factory.TexturesFolderPath]["body"];
                            SpriteBatch.Draw(
                                body,
                                destinationRectangle,
                                Color.White
                            );
                        }
                    }
                }
            }

            //for (int i = -(int)X2; i < X2; i++) // Отрисовка динамических эффектов построек
            //{
            //    for (int j = -(int)Y2; j < Y2; j++)
            //    {
            //        if (
            //            j + (int)WorldPosY >= 0 &&
            //            j + (int)WorldPosY < Field.SizeY &&
            //            i + (int)WorldPosX >= 0 &&
            //            i + (int)WorldPosX < Field.SizeX
            //        )
            //        {
            //            BaseFactory factory = Field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
            //            if (factory != null)
            //            {
            //                Effect effect = factory.DynamicEffect;
            //                if (effect != null)
            //                {
            //                    double nX = WorldPosX * StaticSize % StaticSize; // Получение дробного числа ( дробные.координаты * размер.объекта % размер.объекта )
            //                    double nY = WorldPosY * StaticSize % StaticSize;
            //                    Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
            //                        (int)((i + X2) * StaticSize - nX), // Координата X ( (int)((индекс.поX + половина.масштабаX) * размер.объекта - дробное.число) )
            //                        (int)((j + Y2) * StaticSize - nY), // Координата Y
            //                        (int)(StaticSize + effect.Size),  // Ширина в пикселях
            //                        (int)(StaticSize + effect.Size)   // Высота в пикселях
            //                    );
            //                    Texture2D effect1 = ContentMaster.Textures[factory.TexturePath][effect.TextureName];
            //                    SpriteBatch.Draw(
            //                        effect1,
            //                        destinationRectangle,
            //                        Color.White
            //                    );
            //                }
            //            }
            //        }
            //    }
            //}


                var listName = Field.FieldEquipment["ground"].Keys;
                foreach (string name in listName) // Отрисовка единиц техники
                {
                    var unit = Field.FieldEquipment["ground"][name];
                    if (
                        unit.WorldX >= WorldPosX - X2 - (unit.Size / 2) &&
                        unit.WorldX <= WorldPosX + X2 + (unit.Size / 2) &&
                        unit.WorldY >= WorldPosY - Y2 - (unit.Size / 2) &&
                        unit.WorldY <= WorldPosY + Y2 + (unit.Size / 2)
                    )
                    {
                        Texture2D body = ContentMaster.Textures[unit.TexturesFolderPath]["body"];
                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                            (int)((unit.WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                            (int)(unit.Size * StaticSize),  // Ширина в пикселях
                            (int)(unit.Size * (body.Height / body.Width) * StaticSize)   // Высота в пикселях
                        );
                        SpriteBatch.Draw(
                            body,
                            destinationRectangle,
                            null,
                            Color.White,
                            unit.Rotation,
                            new Vector2(
                                (float)(body.Width / 2 * unit.OffsetX),
                                (float)(body.Height / 2 * unit.OffsetY)
                                ),
                            SpriteEffects.None,
                            0
                        );
                        if (unit.Guns != null)
                        {
                            var gunsList = unit.Guns.Keys;
                            int n = 0;
                            foreach (string nameGun in gunsList) // Отрисовка орудий ( если есть )
                            {
                                Gun gun = unit.Guns[nameGun];
                                Texture2D gunT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/towers"][$"t{n}"];
                                Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                                (int)((unit.WorldX + gun.OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                (int)((unit.WorldY + gun.OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                                (int)(gun.Size * StaticSize),  // Ширина в пикселях
                                (int)(gun.Size * (gunT.Height / gunT.Width) * StaticSize)   // Высота в пикселях
                                );
                                SpriteBatch.Draw(
                                    gunT,
                                    destinationRectangleGuns,
                                    null,
                                    Color.White,
                                    gun.Rotation,
                                    new Vector2(
                                        gunT.Width / 2,
                                        gunT.Height / 2
                                        ),
                                    SpriteEffects.None,
                                    0
                                );
                            n++;
                            }
                        }
                        if (unit.MovingParts != null)
                        {
                            var effectList = unit.MovingParts.Keys;
                            int n = 0;
                            foreach (string nameEffect in effectList) // Отрисовка эффектов ( если есть )
                            {
                                MovingParts effect = unit.MovingParts[nameEffect];
                                Texture2D effectT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/parts"][$"p{n}"];
                                Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                                (int)((unit.WorldX + effect.OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                (int)((unit.WorldY + effect.OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                                (int)(effect.Size * StaticSize),  // Ширина в пикселях
                                (int)(effect.Size * (effectT.Height / effectT.Width) * StaticSize)   // Высота в пикселях
                                );
                                SpriteBatch.Draw(
                                    effectT,
                                    destinationRectangleEffects,
                                    null,
                                    Color.White,
                                    effect.Rotation,
                                    new Vector2(
                                        effectT.Width / 2,
                                        effectT.Height / 2
                                        ),
                                    SpriteEffects.None,
                                    0
                                );
                                n++;
                            }
                        }
                    }
                }

            var airShadowList = Field.FieldEquipment["air"].Keys;
            foreach (string type in airShadowList) // Отрисовка теней воздушной техники
            {
                var unit = Field.FieldEquipment["air"][type];
                float height = unit.Height;
                if (
                    unit.WorldX >= WorldPosX + height - X2 - (unit.Size / 2) &&
                    unit.WorldX <= WorldPosX + height + X2 + (unit.Size / 2) &&
                    unit.WorldY >= WorldPosY - height - Y2 - (unit.Size / 2) &&
                    unit.WorldY <= WorldPosY - height + Y2 + (unit.Size / 2)
                )
                {
                    Texture2D shadow = ContentMaster.Textures[unit.TexturesFolderPath]["shadow"];
                    if (shadow != null)
                    {
                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX - WorldPosX + X2 - height) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                            (int)((unit.WorldY - WorldPosY + Y2 + height) * StaticSize), // Координата Y
                            (int)(unit.Size * StaticSize),  // Ширина в пикселях
                            (int)(unit.Size * (shadow.Height / shadow.Width) * StaticSize)   // Высота в пикселях
                        );
                        SpriteBatch.Draw(
                            shadow,
                            destinationRectangle,
                            null,
                            Color.White,
                            unit.Rotation,
                            new Vector2(
                                (float)(shadow.Width / 2 * unit.OffsetX),
                                (float)(shadow.Height / 2 * unit.OffsetY)
                                ),
                            SpriteEffects.None,
                            0
                        );
                    }
                }
            }

            var airList = Field.FieldEquipment["air"].Keys;
            foreach (string type in airList) // Отрисовка воздушной техники
            {
                var unit = Field.FieldEquipment["air"][type];
                if (
                    unit.WorldX >= WorldPosX - X2 - unit.Size &&
                    unit.WorldX <= WorldPosX + X2 + unit.Size &&
                    unit.WorldY >= WorldPosY - Y2 - unit.Size &&
                    unit.WorldY <= WorldPosY + Y2 + unit.Size
                )
                {
                    Texture2D body = ContentMaster.Textures[unit.TexturesFolderPath]["body"];
                    Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                        (int)((unit.WorldX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                        (int)((unit.WorldY - WorldPosY + Y2) * StaticSize), // Координата Y
                        (int)(unit.Size * StaticSize),  // Ширина в пикселях
                        (int)(unit.Size * (body.Height / body.Width) * StaticSize)   // Высота в пикселях
                    );
                    SpriteBatch.Draw(
                        body,
                        destinationRectangle,
                        null,
                        Color.White,
                        unit.Rotation,
                        new Vector2(
                            (float)(body.Width / 2 * unit.OffsetX),
                            (float)(body.Height / 2 * unit.OffsetY)
                            ),
                        SpriteEffects.None,
                        0
                    );
                    if (unit.Guns != null)
                    {
                        var gunsList = unit.Guns.Keys;
                        int n = 0;
                        foreach (string nameGun in gunsList) // Отрисовка орудий ( если есть )
                        {
                            Gun gun = unit.Guns[nameGun];
                            Texture2D gunT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/towers"][$"t{n}"];
                            Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX + gun.OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                            (int)((unit.WorldY + gun.OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                            (int)(gun.Size * StaticSize),  // Ширина в пикселях
                            (int)(gun.Size * (gunT.Height / gunT.Width) * StaticSize)   // Высота в пикселях
                            );
                            SpriteBatch.Draw(
                                gunT,
                                destinationRectangleGuns,
                                null,
                                Color.White,
                                gun.Rotation,
                                new Vector2(
                                    gunT.Width / 2,
                                    gunT.Height / 2
                                    ),
                                SpriteEffects.None,
                                0
                            );
                            n++;
                        }
                    }
                    if (unit.MovingParts != null)
                    {
                        var effectList = unit.MovingParts.Keys;
                        int n = 0;
                        foreach (string nameEffect in effectList) // Отрисовка эффектов ( если есть )
                        {
                            MovingParts effect = unit.MovingParts[nameEffect];
                            Texture2D effectT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/parts"][$"p{n}"];
                            Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX + effect.OffsetX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                            (int)((unit.WorldY + effect.OffsetY - WorldPosY + Y2) * StaticSize), // Координата Y
                            (int)(effect.Size * StaticSize),  // Ширина в пикселях
                            (int)(effect.Size * (effectT.Height / effectT.Width) * StaticSize)   // Высота в пикселях
                            );
                            SpriteBatch.Draw(
                                effectT,
                                destinationRectangleEffects,
                                null,
                                Color.White,
                                effect.Rotation,
                                new Vector2(
                                    effectT.Width / 2,
                                    effectT.Height / 2
                                    ),
                                SpriteEffects.None,
                                0
                            );
                            n++;
                        }
                    }
                }
            }
        }
    }
}
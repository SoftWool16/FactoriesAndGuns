using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace Factories_And_Guns
{
    internal class MatrixCamera
    {
        static public float WorldPosX { get; set; } = 0; // Координаты камеры
        static public float WorldPosY { get; set; } = 0;
        static public float SizeX { get; set; } = 60; // Кол-во блоков, которых нужно отрисовать по X
        static public float SizeY { get; set; } = 30; // Кол-во блоков, которых нужно отрисовать по Y
        static public float StaticSize { get; set; } = 50; // Масштаб
        static public float SizeVector { get; set; } = 1;
        static public float CameraSpeed { get; set; } = 0.01f;
        static public float CameraSpeedZoom { get; set; } = 0.01f;
        static public float MinZoom { get; set; } = 60;
        static public float MaxZoom { get; set; } = 350;

        static public void CameraUpdate()
        {
            if (SizeX * SizeVector < MaxZoom &&
                SizeX * SizeVector > MinZoom
                ) SizeX *= SizeVector;
            else SizeVector = 1;

            if (SizeVector > 1 + CameraSpeedZoom) SizeVector -= CameraSpeedZoom;
            else if (SizeVector < 1 - CameraSpeedZoom) SizeVector += CameraSpeedZoom;
            else SizeVector = 1;
        }
        static public void RenderMatrix(GameWindow gameWindow, SpriteBatch spriteBatch)
        {
            Field field = General.CurrentSunSystem?.CurrentPlanet?.CurrentField;

            if (field != null)
            {
                StaticSize = gameWindow.ClientBounds.Width / SizeX; // Установка масштаба объектов с учётом кол-ва блоков, которых нужно отрисовать по X ( ширина_окна_в_пикселях / кол-во_блоков.по_X )
                SizeY = gameWindow.ClientBounds.Height / StaticSize;

                float X2 = SizeX / 2;
                float Y2 = SizeY / 2;

                for (int i = -(int)X2 - 1; i < X2 + 1; i++) // Отрисовка фона
                {
                    for (int j = -(int)Y2 - 1; j < Y2 + 1; j++)
                    {
                        if (
                            j + (int)WorldPosY >= 0 &&
                            j + (int)WorldPosY < field.SizeY &&
                            i + (int)WorldPosX >= 0 &&
                            i + (int)WorldPosX < field.SizeX
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

                            Texture2D texture;
                            try
                            {
                                texture = ContentMaster.Textures[
                                        field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].TexturesFolderPath
                                        ][
                                        field.FieldBackground[j + (int)WorldPosY, i + (int)WorldPosX].Name
                                        ];
                            }
                            catch
                            {
                                texture = ContentMaster.Textures[""]["mistake"];
                            }

                            spriteBatch.Draw(
                                texture,
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
                            j + (int)WorldPosY < field.SizeY &&
                            i + (int)WorldPosX >= 0 &&
                            i + (int)WorldPosX < field.SizeX
                        )
                        {
                            BaseFactory factory = field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
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

                                Texture2D shadow;
                                try
                                {
                                    shadow = ContentMaster.Textures["Block"]["shadow"];
                                }
                                catch
                                {
                                    shadow = ContentMaster.Textures[""]["mistake"];
                                }

                                spriteBatch.Draw(
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
                            j + (int)WorldPosY < field.SizeY &&
                            i + (int)WorldPosX >= 0 &&
                            i + (int)WorldPosX < field.SizeX
                        )
                        {
                            BaseFactory factory = field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
                            if (factory != null)
                            {
                                int n = 0;
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

                                    Texture2D effect1;
                                    try
                                    {
                                        effect1 = ContentMaster.Textures[factory.TexturesFolderPath][$"part{n}"];
                                    }
                                    catch
                                    {
                                        effect1 = ContentMaster.Textures[""]["mistake"];
                                    }

                                    spriteBatch.Draw(
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
                                    n++;
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
                            j + (int)WorldPosY < field.SizeY &&
                            i + (int)WorldPosX >= 0 &&
                            i + (int)WorldPosX < field.SizeX
                        )
                        {
                            BaseFactory factory = field.FieldBuild[j + (int)WorldPosY, i + (int)WorldPosX];
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

                                Texture2D body;
                                try
                                {
                                    body = ContentMaster.Textures[factory.TexturesFolderPath]["body"];
                                }
                                catch
                                {
                                    body = ContentMaster.Textures[""]["mistake"];
                                }

                                spriteBatch.Draw(
                                    body,
                                    destinationRectangle,
                                    Color.White
                                );
                            }
                        }
                    }
                }

                var typeList = field.FieldEquipment.Keys;
                foreach (var type in typeList)          // Отрисовка полутени (для контраста техники)
                {
                    var nameList = field.FieldEquipment[type].Keys;
                    foreach (var name in nameList)
                    {
                        var unit = field.FieldEquipment[type][name];
                        if (
                        unit.WorldX >= WorldPosX - X2 - (unit.Size * 1.15 / 2) &&
                        unit.WorldX <= WorldPosX + X2 + (unit.Size * 1.15 / 2) &&
                        unit.WorldY >= WorldPosY - Y2 - (unit.Size * 1.15 / 2) &&
                        unit.WorldY <= WorldPosY + Y2 + (unit.Size * 1.15 / 2)
                        )
                        {
                            Texture2D partialShade;
                            try
                            {
                                partialShade = ContentMaster.Textures[unit.TexturesFolderPath]["partial_shade"];
                            }
                            catch
                            {
                                partialShade = ContentMaster.Textures[""]["mistake"];
                            }

                            int X = (int)((unit.WorldX - WorldPosX + X2) * StaticSize);
                            int Y = (int)((unit.WorldY - WorldPosY + Y2) * StaticSize);

                            Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                                X, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                                Y, // Координата Y
                                (int)(unit.Size * StaticSize * 1.15),  // Ширина в пикселях
                                (int)(unit.Size * ((float)partialShade.Height / partialShade.Width) * StaticSize * 1.15)   // Высота в пикселях
                            );

                            unit.ScreenX = X; unit.ScreenY = Y;

                            spriteBatch.Draw(
                                partialShade,
                                destinationRectangle,
                                null,
                                Color.White,
                                unit.Rotation,
                                new Vector2(
                                    (float)(partialShade.Width / 2 * unit.OffsetCenterX),
                                    (float)(partialShade.Height / 2 * unit.OffsetCenterY)
                                    ),
                                SpriteEffects.None,
                                0
                            );
                        }

                    }
                }

                var listName = field.FieldEquipment["ground"].Keys;
                foreach (string name in listName) // Отрисовка единиц техники
                {
                    var unit = field.FieldEquipment["ground"][name];
                    if (
                        unit.WorldX >= WorldPosX - X2 - (unit.Size / 2) &&
                        unit.WorldX <= WorldPosX + X2 + (unit.Size / 2) &&
                        unit.WorldY >= WorldPosY - Y2 - (unit.Size / 2) &&
                        unit.WorldY <= WorldPosY + Y2 + (unit.Size / 2)
                    )
                    {
                        Texture2D body;
                        try
                        {
                            body = ContentMaster.Textures[unit.TexturesFolderPath]["body"];
                        }
                        catch
                        {
                            body = ContentMaster.Textures[""]["mistake"];
                        }

                        int X = (int)((unit.WorldX - WorldPosX + X2) * StaticSize);
                        int Y = (int)((unit.WorldY - WorldPosY + Y2) * StaticSize);

                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            X, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                            Y, // Координата Y
                            (int)(unit.Size * StaticSize),  // Ширина в пикселях
                            (int)(unit.Size * ((float)body.Height / body.Width) * StaticSize)   // Высота в пикселях
                        );

                        unit.ScreenX = X; unit.ScreenY = Y;

                        spriteBatch.Draw(
                            body,
                            destinationRectangle,
                            null,
                            Color.White,
                            unit.Rotation,
                            new Vector2(
                                (float)(body.Width / 2 * unit.OffsetCenterX),
                                (float)(body.Height / 2 * unit.OffsetCenterY)
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
                                if (gun != null)
                                {
                                    Texture2D gunT;
                                    try
                                    {
                                        gunT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/towers"][$"t{n}"];
                                    }
                                    catch
                                    {
                                        gunT = ContentMaster.Textures[""]["mistake"];
                                    }

                                    int gX = (int)((unit.WorldX + gun.OffsetPosX - WorldPosX + X2) * StaticSize);
                                    int gY = (int)((unit.WorldY + gun.OffsetPosY - WorldPosY + Y2) * StaticSize);

                                    Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                                    gX, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                    gY, // Координата Y
                                    (int)(gun.Size * StaticSize),  // Ширина в пикселях
                                    (int)(gun.Size * ((float)gunT.Height / gunT.Width) * StaticSize)   // Высота в пикселях
                                    );

                                    gun.ScreenX = gX; gun.ScreenY = gY;

                                    spriteBatch.Draw(
                                        gunT,
                                        destinationRectangleGuns,
                                        null,
                                        Color.White,
                                        gun.Rotation,
                                        new Vector2(
                                            gunT.Width / 2 * gun.OffsetCenterX,
                                            gunT.Height / 2 * gun.OffsetCenterY
                                            ),
                                        SpriteEffects.None,
                                        0
                                    );
                                }
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
                                if (effect != null)
                                {
                                    Texture2D effectT;
                                    try
                                    {
                                        effectT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/parts"][$"p{n}"];
                                    }
                                    catch
                                    {
                                        effectT = ContentMaster.Textures[""]["mistake"];
                                    }

                                    int eX = (int)((unit.WorldX + effect.OffsetPosX - WorldPosX + X2) * StaticSize);
                                    int eY = (int)((unit.WorldY + effect.OffsetPosY - WorldPosY + Y2) * StaticSize);

                                    Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                                    eX, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                    eY, // Координата Y
                                    (int)(effect.Size * StaticSize),  // Ширина в пикселях
                                    (int)(effect.Size * ((float)effectT.Height / effectT.Width) * StaticSize)   // Высота в пикселях
                                    );

                                    effect.ScreenX = eX; effect.ScreenY = eY;

                                    spriteBatch.Draw(
                                        effectT,
                                        destinationRectangleEffects,
                                        null,
                                        Color.White,
                                        effect.Rotation,
                                        new Vector2(
                                            effectT.Width / 2 * effect.OffsetCenterX,
                                            effectT.Height / 2 * effect.OffsetCenterY
                                            ),
                                        SpriteEffects.None,
                                        0
                                    );
                                }
                                n++;
                            }
                        }
                    }
                }

                var airShadowList = field.FieldEquipment["air"].Keys;
                foreach (string type in airShadowList) // Отрисовка теней воздушной техники
                {
                    var unit = field.FieldEquipment["air"][type];
                    float height = unit.Height;
                    if (
                        unit.WorldX >= WorldPosX + height - X2 - (unit.Size / 2) &&
                        unit.WorldX <= WorldPosX + height + X2 + (unit.Size / 2) &&
                        unit.WorldY >= WorldPosY - height - Y2 - (unit.Size / 2) &&
                        unit.WorldY <= WorldPosY - height + Y2 + (unit.Size / 2)
                    )
                    {
                        Texture2D shadow;
                        try
                        {
                            shadow = ContentMaster.Textures[unit.TexturesFolderPath]["shadow"];
                        }
                        catch
                        {
                            shadow = ContentMaster.Textures[""]["mistake"];
                        }

                        float h = 1 - (unit.Height / 10);
                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            (int)((unit.WorldX - WorldPosX + X2 - height) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                            (int)((unit.WorldY - WorldPosY + Y2 + height) * StaticSize), // Координата Y
                            (int)(unit.Size * StaticSize * h),  // Ширина в пикселях
                            (int)(unit.Size * ((float)shadow.Height / shadow.Width) * StaticSize * h)   // Высота в пикселях
                        );

                        spriteBatch.Draw(
                            shadow,
                            destinationRectangle,
                            null,
                            Color.White,
                            unit.Rotation,
                            new Vector2(
                                (float)(shadow.Width / 2 * unit.OffsetCenterX),
                                (float)(shadow.Height / 2 * unit.OffsetCenterY)
                                ),
                            SpriteEffects.None,
                            0
                        );
                    }
                }

                var airList = field.FieldEquipment["air"].Keys;
                foreach (string type in airList) // Отрисовка воздушной техники
                {
                    var unit = field.FieldEquipment["air"][type];
                    if (
                        unit.WorldX >= WorldPosX - X2 - unit.Size &&
                        unit.WorldX <= WorldPosX + X2 + unit.Size &&
                        unit.WorldY >= WorldPosY - Y2 - unit.Size &&
                        unit.WorldY <= WorldPosY + Y2 + unit.Size
                    )
                    {
                        Texture2D body;
                        try
                        {
                            body = ContentMaster.Textures[unit.TexturesFolderPath]["body"];
                        }
                        catch
                        {
                            body = ContentMaster.Textures[""]["mistake"];
                        }

                        int X = (int)((unit.WorldX - WorldPosX + X2) * StaticSize);
                        int Y = (int)((unit.WorldY - WorldPosY + Y2) * StaticSize);

                        Rectangle destinationRectangle = new( // 4-х угольник, на который будет надета текстура
                            X, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта) )
                            Y, // Координата Y
                            (int)(unit.Size * StaticSize),  // Ширина в пикселях
                            (int)(unit.Size * ((float)body.Height / body.Width) * StaticSize)   // Высота в пикселях
                        );

                        unit.ScreenX = X; unit.ScreenY = Y;

                        spriteBatch.Draw(
                            body,
                            destinationRectangle,
                            null,
                            Color.White,
                            unit.Rotation,
                            new Vector2(
                                (float)(body.Width / 2 * unit.OffsetCenterX),
                                (float)(body.Height / 2 * unit.OffsetCenterY)
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
                                if (gun != null)
                                {
                                    Texture2D gunT;
                                    try
                                    {
                                        gunT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/towers"][$"t{n}"];
                                    }
                                    catch
                                    {
                                        gunT = ContentMaster.Textures[""]["mistake"];
                                    }

                                    int gX = (int)((unit.WorldX + gun.OffsetPosX - WorldPosX + X2) * StaticSize);
                                    int gY = (int)((unit.WorldY + gun.OffsetPosY - WorldPosY + Y2) * StaticSize);

                                    Rectangle destinationRectangleGuns = new( // 4-х угольник, на который будет надета текстура
                                    gX, // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                    gY, // Координата Y
                                    (int)(gun.Size * StaticSize),  // Ширина в пикселях
                                    (int)(gun.Size * ((float)gunT.Height / gunT.Width) * StaticSize)   // Высота в пикселях
                                    );

                                    unit.ScreenX = gX; unit.ScreenY = gY;

                                    spriteBatch.Draw(
                                        gunT,
                                        destinationRectangleGuns,
                                        null,
                                        Color.White,
                                        gun.Rotation,
                                        new Vector2(
                                            gunT.Width / 2 * gun.OffsetCenterX,
                                            gunT.Height / 2 * gun.OffsetCenterY
                                            ),
                                        SpriteEffects.None,
                                        0
                                    );
                                }
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
                                if (effect != null)
                                {
                                    Texture2D effectT;
                                    try
                                    {
                                        effectT = ContentMaster.Textures[$"{unit.TexturesFolderPath}/parts"][$"p{n}"];
                                    }
                                    catch
                                    {
                                        effectT = ContentMaster.Textures[""]["mistake"];
                                    }

                                    Rectangle destinationRectangleEffects = new( // 4-х угольник, на который будет надета текстура
                                    (int)((unit.WorldX + effect.OffsetPosX - WorldPosX + X2) * StaticSize), // Координата X ( (int)((координатаОбъекта.поX - координатаКамеры.поX + половина.масштабаX) * размер.объекта * отклонение.поX) )
                                    (int)((unit.WorldY + effect.OffsetPosY - WorldPosY + Y2) * StaticSize), // Координата Y
                                    (int)(effect.Size * StaticSize),  // Ширина в пикселях
                                    (int)(effect.Size * ((float)effectT.Height / effectT.Width) * StaticSize)   // Высота в пикселях
                                    );

                                    spriteBatch.Draw(
                                        effectT,
                                        destinationRectangleEffects,
                                        null,
                                        Color.White,
                                        effect.Rotation,
                                        new Vector2(
                                            effectT.Width / 2 * effect.OffsetCenterX,
                                            effectT.Height / 2 * effect.OffsetCenterY
                                            ),
                                        SpriteEffects.None,
                                        0
                                    );
                                }
                                n++;
                            }
                        }
                    }
                }
            }

            var ind = Interface.Templates["field"].Keys;
            foreach (string name in ind)
            {
                var element = Interface.Templates["field"][name];
                if (element != null)
                {
                    Texture2D elementT;
                    try
                    {
                        elementT = ContentMaster.Textures[element.TexturesFolderPath][element.Name];
                    }
                    catch
                    {
                        elementT = ContentMaster.Textures[""]["mistake"];
                    }

                    spriteBatch.Draw(
                        elementT,
                        element.Rectangle,
                        null,
                        Color.White,
                        element.Rotation,
                        new(elementT.Height / 2, elementT.Width / 2),
                        SpriteEffects.None,
                        0
                    );
                }
            }

            string current = Interface.CurrentTemplate[Interface.CurrentTemplateIndex];
            var top = Interface.Templates[current].Keys;
            foreach (string name in top)
            {
                var element = Interface.Templates[current][name];
                if (element != null)
                {
                    Texture2D elementT;
                    try
                    {
                        elementT = ContentMaster.Textures[element.TexturesFolderPath][element.Name];
                    }
                    catch
                    {
                        elementT = ContentMaster.Textures[""]["mistake"];
                    }

                    spriteBatch.Draw(
                        elementT,
                        element.Rectangle,
                        null,
                        Color.White,
                        element.Rotation,
                        new(elementT.Height / 2, elementT.Width / 2),
                        SpriteEffects.None,
                        0
                    );
                }
            }

            var cursor = Interface.Cursor;

            Texture2D cursorT;
            try
            {
                cursorT = ContentMaster.Textures[cursor.TexturesFolderPath][cursor.Name];
            }
            catch
            {
                cursorT = ContentMaster.Textures[""]["mistake"];
            }

            spriteBatch.Draw(
                cursorT,
                cursor.Rectangle,
                Color.White
            );
            
        }
    }
}
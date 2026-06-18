using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    public class Element(string Name, string texturePath)
    {
        public string Name { get; set; } = Name;
        public string TexturesFolderPath { get; set; } = texturePath;
    }
    public class InterfaceElement(int x, int y, string name, string path, int size, bool foolScreen, string interfaceForOpen) : Element(name, path)
    {
        public Rectangle Rectangle { get; set; } = Rectangle.Empty;

        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Size { get; set; } = size;
        public float Rotation { get; set; } = 0;
        public bool FoolScreen { get; set; } = foolScreen;
        public string InterfaceForOpen { get; set; } = interfaceForOpen;

        public void Update(MouseState mouse)
        {
            bool IsNotFalse = false;

            var list1 = ContentMaster.Textures.Keys;
            foreach (string Path in list1)
            {
                if (TexturesFolderPath == Path)
                {
                    var list2 = ContentMaster.Textures[Path].Keys;
                    foreach (string Name in list2)
                    {
                        if (this.Name == Name) { IsNotFalse = true; break; }
                    }
                }
            }

            if (IsNotFalse)
            {
                Texture2D texture2D = ContentMaster.Textures[TexturesFolderPath][Name];

                if (FoolScreen == false) Rectangle = new(X, Y, Size, (int)(Size * ((float)texture2D.Height / texture2D.Width)));
            }
            else
            {
                if (FoolScreen == false) Rectangle = new(X, Y, Size, Size);
            }

            if (InterfaceForOpen != null)
            {
                bool isInside = Rectangle.Contains(mouse.Position);

                if (isInside && mouse.LeftButton == ButtonState.Pressed)
                {
                    FocusClass.IsFocused = true;

                    FocusClass.Decreasing = true;

                    var list = Interface.Templates.Keys;
                    foreach (string name in list)
                    {
                        if (name == InterfaceForOpen)
                        {
                            Interface.CurrentTemplateIndex += 1;
                            Interface.CurrentTemplate[Interface.CurrentTemplateIndex] = InterfaceForOpen;
                            break;
                        }
                    }
                }

                else if (isInside)
                {
                    FocusClass.IsFocused = true;

                    Interface.MoveFocusAndResize(X + (Rectangle.Width / 2), Y + (Rectangle.Height / 2), Size);
                }
            }
        }
    }
    public class Parameters(float size, float offsetCenterX, float offsetCenterY, float offsetPosX, float offsetPosY, float maxSpeedRotation)
    {
        public float Rotation { get; set; } = 0;
        public float WorldX { get; set; } = 0;
        public float WorldY { get; set; } = 0;
        public int ScreenX { get; set; } = 0;
        public int ScreenY { get; set; } = 0;
        public float Size { get; set; } = size;
        public float OffsetCenterX { get; set; } = offsetCenterX;
        public float OffsetCenterY { get; set; } = offsetCenterY;
        public float OffsetPosX { get; set; } = offsetPosX;
        public float OffsetPosY { get; set; } = offsetPosY;
        public float MaxSpeedRotation { get; set; } = maxSpeedRotation;

        public Vector2 DirectionFromAngle() // получение вектора движения из поворота
        {
            // Если нужно, «обернём» угол в диапазон [-p, p] – это не обязательно,  ( здесь p - число пи )
            // но убирает лишние большие значения.
            //Rotation = MathHelper.WrapAngle(Rotation);

            float y = MathF.Cos(Rotation); // [-1, 1]
            float x = MathF.Sin(Rotation); // [-1, 1]

            // Косинус/синус уже дают единичный вектор, но можно гарантировать нормализацию:
            // return Vector2.Normalize(new Vector2(x, y));
            return new Vector2(x, -y);
        }
        public void SmoothRotation(Vector2 vector2, float dt) // Плавный поворот
        {
            // Зависит от того, куда смотрит "нос" вашего спрайта по умолчанию.
            // Если вверх: MathF.Atan2(move.X, -move.Y)
            // Если вправо: MathF.Atan2(move.Y, move.X)
            float atan = MathF.Atan2(vector2.X, -vector2.Y);

            // Кратчайшее отклонение
            float angleDiff = MathHelper.WrapAngle(atan - Rotation);

            // Ограничиваем шаг за кадр
            float maxStep = MaxSpeedRotation * dt;
            float step = MathHelper.Clamp(angleDiff, -maxStep, maxStep);

            //Обновляем угол и снова «заворачиваем», чтобы он оставался в [-p, p] ( здесь p - число пи )
            Rotation = MathHelper.WrapAngle(Rotation + step);
        }
    }

    public enum MovingPartsType
    {
        rotation,
        shaking,
        movement
    }

    public enum EquipmentMoveType
    {
        tracked,
        walking,
        hovering,
        swimming
    }

    public class TypeBullet(float size, float maxSpeedRotation) : Parameters(size, 0, 0, 0, 0, maxSpeedRotation)
    {
        public void BulletUpdate(float dt)
        {
            
        }
    }
    public class Gun(float offsetCenterX, float offsetCenterY, float offsetPosX, float offsetPosY, int damage, TypeBullet bullet, float rate_of_fire, float speedBullet, float speedRotation, float size, float maxBackfiring, float backfiring) : Parameters(size, offsetCenterX, offsetCenterY, offsetPosX, offsetPosY, speedRotation)
    {
        public int Damage { get; set; } = damage;
        public TypeBullet _TypeBullet { get; set; } = bullet;
        public float Rate_of_fire { get; set; } = rate_of_fire;
        public float SpeedBullet {get; set;} = speedBullet;
        public float MaxBackfiring {  get; set; } = maxBackfiring;
        public float Backfiring { get; set; } = backfiring;
        public Vector2 CurrentBackfiring { get; set; } = Vector2.Zero;

        public void GunUpdate(float dt)
        {
            //if (Backfiring <= MaxBackfiring - Backfiring) 
        }
    }

    public class MovingParts (float offsetCenterX, float offsetCenterY, float offsetPosX, float offsetPosY, float speedEffect, MovingPartsType movingPartsType, float size) : Parameters(size, offsetCenterX, offsetCenterY, offsetPosX, offsetPosY, 0)
    {
        public float SpeedEffect {  get; set; } = speedEffect;
        public MovingPartsType MovingPartsType { get; set; } = movingPartsType;
        public void MovingPartsUpdate(float dt)
        {
            if (MovingPartsType == MovingPartsType.rotation)
            {
                float r = Rotation + SpeedEffect * dt;
                if (r >= 360)
                {
                    float r1 = 360 - r;
                    Rotation = r1;
                }
                else Rotation = r;
            }
            //else if (EffectType == EffectType.shaking) 
        }
    }

    public class Equipment : Parameters
    {
        public Equipment(float offsetCenterX, float offsetCenterY, float offsetPosX, float offsetPosY, float size, string name, float X, float Y, string textureFolderPath, Dictionary<string, Gun> guns, Dictionary<string, MovingParts> movingParts, float maxSpeed, float acceleration, float friction, float maxSpeedRotation, float maxHealth, EquipmentMoveType moveType, int maxHeightToBeOvercome, int maxDepthToBeOvercome) : base(size, offsetCenterX, offsetCenterY, offsetPosX, offsetPosY, maxSpeedRotation)
        {
            Health = maxHealth;
            WorldX = X;
            WorldY = Y;
            Guns = guns;
            MovingParts = movingParts;
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Friction = friction;
            MoveType = moveType;
            MaxHeightToBeOvercome = maxHeightToBeOvercome;
            MaxDepthToBeOvercome = maxDepthToBeOvercome;
            NameEquipment = name;
            TexturesFolderPath = textureFolderPath;
            Type = "ground";
        }

        public Equipment(float offsetCenterX, float offsetCenterY, float offsetPosX, float offsetPosY, float size, string name, float X, float Y, string textureFolderPath, Dictionary<string, Gun> guns, Dictionary<string, MovingParts> movingParts, float maxSpeed, float acceleration, float maxSpeedRotation, float maxHealth, EquipmentMoveType moveType) : base(size, offsetCenterX, offsetCenterY, offsetPosX, offsetPosY, maxSpeedRotation)
        {
            Health = maxHealth;
            WorldX = X;
            WorldY = Y; 
            Guns = guns;
            MovingParts = movingParts;
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Friction = acceleration;
            MoveType = moveType;
            MaxHeightToBeOvercome = -1;
            MaxDepthToBeOvercome = -1;
            NameEquipment = name;
            TexturesFolderPath = textureFolderPath;
            Type = "air";
        }

        public string Type { get; set; } // Тип
        public float Health { get; set; }  // Текущее здоровье
        public string NameEquipment { get; set; } // Игровое имя
        public string TexturesFolderPath { get; set; } // Путь к папке с текстурами
        public Dictionary<string, Gun> Guns { get; set; }  // Список орудий
        public Dictionary<string, MovingParts> MovingParts { get; set; }  // Список постоянных эффектов
        public float MaxSpeed { get; set; }  // Максимальная скорость передвижения
        public Vector2 Velocity { get; set; } = Vector2.Zero; // Текущая скорость
        public float Acceleration { get; set; }  // Ускорение
        public float Friction { get; set; }      // Замедление
        public EquipmentMoveType MoveType { get; set; }  // Тип передвижения
        public float WorldPointX { get; set; } = -1;  // Точка X для перемещения в неё
        public float WorldPointY { get; set; } = -1;  // Точка Y для перемещения в неё

        public int MaxHeightToBeOvercome { get; set; } // Максимальная проходимая высота
        public int MaxDepthToBeOvercome { get; set; } // Максимальная проходимая глубина

        public float Height { get; set; } = 3; // Высота полёта ( для воздушных )

        public void InputHalderEquipment(Keys[] keys, MouseState mouse, float dt)
        {
            // Направляющий вектор ( (0,0) если нет ввода )
            Vector2 move = Vector2.Zero;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == Keys.W) move.Y -= 1;
                if (keys[i] == Keys.S) move.Y += 1;
                if (keys[i] == Keys.A) move.X -= 1;
                if (keys[i] == Keys.D) move.X += 1;
            }

            if (move != Vector2.Zero)
            {
                Vector2 vector2 = move;

                if (MoveType == EquipmentMoveType.tracked) vector2 = DirectionFromAngle();

                // Нормализуем, чтобы по диагонали скорость была такой же, как по осям
                vector2 = Vector2.Normalize(vector2);

                // Текущее направление скорости (нормализованное, если модуль > 0)
                Vector2 velDir = Velocity;
                float velLen = velDir.Length();
                if (velLen > 0f) velDir /= velLen;    // единичный вектор

                // Скалярное произведение: >0 – одно направление, <0 – противоположное
                float dot = Vector2.Dot(vector2, velDir);

                if (dot >= 0f)                     // двигаемся в ту же сторону -> ускоряем
                {
                    Velocity += vector2 * Acceleration * dt;
                }
                else                               // нажали «против» -> тормозим
                {
                    // Уменьшаем модуль скорости торможением
                    float newLen = MathF.Max(0f, velLen - Friction * dt);

                    // Если полностью остановились – сразу начинаем ускоряться в нужном направлении
                    if (newLen == 0f)
                        Velocity = vector2 * Acceleration * dt;
                    else
                        Velocity = velDir * newLen;
                }

                // Ограничиваем модуль скорости
                if (Velocity.Length() > MaxSpeed)
                {
                    Velocity = Vector2.Normalize(Velocity)
                                                   * MaxSpeed;
                }

                // Поворот к направлению движения
                SmoothRotation(move, dt);
            }
            else
            {
                SmoothStop(dt);
            }

            MatrixCamera.WorldPosX = WorldX;
            MatrixCamera.WorldPosY = WorldY;
        }
        public void SmoothStop(float dt)
        {
            if (Velocity.Length() > 0f)
            {
                // Уменьшаем модуль скорости
                float speed = Velocity.Length();
                speed -= Friction * dt;
                speed = Math.Max(speed, 0f);

                // Сохраняем направление
                if (speed == 0f)
                    Velocity = Vector2.Zero;
                else
                    Velocity = Vector2.Normalize(Velocity) * speed;
            }
        }
        public void Update(float dt, bool x, bool y, Vector2 mousePos)
        {
            if (MovingParts != null) // Обновление эффектов
            {
                var unitEffectList = MovingParts.Keys;
                foreach (var effect in unitEffectList) MovingParts[effect].MovingPartsUpdate(dt);
            }

            // Обновление движения
            if (Velocity != Vector2.Zero && x == true) WorldX += Velocity.X * dt;
            else Velocity = new Vector2(0, Velocity.Y);
            if (Velocity != Vector2.Zero && y == true) WorldY += Velocity.Y * dt;
            else Velocity = new Vector2(Velocity.X, 0);

            if (mousePos != Vector2.Zero)
            {
                // Обработка орудий
                if (Guns != null)
                {
                    foreach (string gun in Guns.Keys)
                    {
                        mousePos = new(mousePos.X - Guns[gun].ScreenX, mousePos.Y - Guns[gun].ScreenY);
                        Guns[gun].SmoothRotation(mousePos, dt);
                    }
                }
            }
        }
    }

    public class BaseFactory(string Name, string bodyTextureName, int size, MovingParts movingParts, MovingParts dynamicEffect, Dictionary<string, Element> elementsIn, Dictionary<string, Element> elementsOut, float maxHealth, int capacity) : Element(Name, bodyTextureName)
    {
        public int Capacity { get; set; } = capacity;
        public float Health { get; set; } = maxHealth;
        public int Size { get; set; } = size;
        public MovingParts MovingParts { get; set; } = movingParts;
        public MovingParts DynamicEffect { get; set; } = dynamicEffect;
        public Dictionary<string, Element> ElementsIn { get; set; } = elementsIn;
        public Dictionary <string, Element> ElementsOut { get; set; } = elementsOut;
        public Dictionary<string, Element> Elements { get; set; } = null;
    }

    public class FocusClass
    {
        static public InterfaceElement Focus { get; set; } = new(0, 0, "focus", "User_Interface", 80, false, null);
        static public bool IsFocused { get; set; } = false;
        static public float FocusMoveSpeed { get; set; } = 30;
        static public float FocusRotationSpeed { get; set; } = 0.07f;
        static public int MinSize { get; set; } = 40;
        static public int MaxSize { get; set; } = 200;
        static public int FocusGrowthSpeed { get; set; } = 20;
        static public bool Decreasing { get; set; } = false;

        static public void Update(MouseState mouse)
        {
            Focus.Update(mouse);

            if (Decreasing)
            {
                if (Focus.Size > 0) Focus.Size -= FocusGrowthSpeed;
                else Decreasing = false;
            }

            if (Focus.Rotation < 360) Focus.Rotation += FocusRotationSpeed;
            else Focus.Rotation = 0;
        }
    }

    public class Interface
    {
        static public Dictionary<string, Dictionary<string, InterfaceElement>> Templates { get; set; } = [];
        static public string[] CurrentTemplate { get; set; } = new string[10];
        static public int CurrentTemplateIndex { get; set; } = 0;
        static public InterfaceElement Cursor { get; set; } = new(0, 0, "base", "User_Interface/cursors", 30, false, null);
        
        static public Dictionary<string, Keys> Keys { get; set; } = [];

        static public void Update(Keys[] keys, MouseState mouse)
        {
            FocusClass.IsFocused = false;

            string current = CurrentTemplate[CurrentTemplateIndex];
            var top = Templates[current].Keys;
            foreach (string name in top) Templates[current][name].Update(mouse);

            Cursor.Update(mouse);
            FocusClass.Update(mouse);

            var keysList = Keys.Keys;
            for (int i = 0; i < keys.Length; i++)
            {
                foreach (var key in keysList)
                {
                    if (keys[i] == Keys[key] && 
                        CurrentTemplate[CurrentTemplateIndex] != key && 
                        CurrentTemplateIndex < CurrentTemplate.Length)
                    {
                        CurrentTemplateIndex += 1;
                        CurrentTemplate[CurrentTemplateIndex] = key;
                    }

                    if (keys[i] == Keys["back"] && CurrentTemplateIndex > 0)
                    {
                        CurrentTemplateIndex -= 1;
                    }

                    if (keys[i] == Keys["home"]) CurrentTemplateIndex = 0;
                }
            }
        }

        static public void MoveFocusAndResize(int x, int y, int Size)
        {
            int X = x - FocusClass.Focus.X;
            int Y = y - FocusClass.Focus.Y;

            Vector2 vector2 = new(X, Y);

            vector2 = Vector2.Normalize(vector2);

            if (FocusClass.Focus.X != x || FocusClass.Focus.Y != y)
            {
                float focusMoveSpeed = FocusClass.FocusMoveSpeed;
                if (FocusClass.Focus.X + (int)(vector2.X * focusMoveSpeed) > x &&
                    FocusClass.Focus.X - (int)(vector2.X * focusMoveSpeed) < x) FocusClass.Focus.X = x;
                else FocusClass.Focus.X += (int)(vector2.X * focusMoveSpeed);

                if (FocusClass.Focus.Y + (int)(vector2.Y * focusMoveSpeed) > y &&
                    FocusClass.Focus.Y - (int)(vector2.Y * focusMoveSpeed) < y) FocusClass.Focus.Y = y;
                else FocusClass.Focus.Y += (int)(vector2.Y * focusMoveSpeed);
            }

            if (!FocusClass.Decreasing)
            {
                if (FocusClass.Focus.Size < Size * 2 && FocusClass.Focus.Size < FocusClass.MaxSize) FocusClass.Focus.Size += FocusClass.FocusGrowthSpeed;
                else if (FocusClass.Focus.Size > Size * 2 && FocusClass.Focus.Size > FocusClass.MinSize) FocusClass.Focus.Size -= FocusClass.FocusGrowthSpeed;
            }
        }
    }
}
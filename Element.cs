using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Factories_And_Guns
{
    public class Element(string name, string texturePath)
    {
        public string Name { get; set; } = name;
        public string TexturesFolderPath { get; set; } = texturePath;
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

    public class Bullet(string textureName, float size)
    {
        public string TextureName { get; set; } = textureName;
        public float Rotation { get; set; } = 0;
        public float WorldX { get; set; } = 0;
        public float WorldY { get; set; } = 0;
        public float Size { get; set; } = size;
        public void BulletUpdate(float dt)
        {
            
        }
    }
    public class Gun(string textureName, float offsetCenterX, float offsetCenterY, int damage, Bullet bullet, float rate_of_fire, float speedBullet, float speedRotation, float size)
    {
        public string TextureName { get; set; } = textureName;
        public float Rotation { get; set; } = 0;
        public float OffsetX { get; set; } = offsetCenterX;
        public float OffsetY { get; set; } = offsetCenterY;
        public int Damage { get; set; } = damage;
        public Bullet Bullet { get; set; } = bullet;
        public float Rate_of_fire { get; set; } = rate_of_fire;
        public float SpeedBullet {get; set;} = speedBullet;
        public float SpeedRotation { get; set;} = speedRotation;
        public float Size { get; set; } = size;
    }

    public class MovingParts (string textureName, float offsetCenterX, float offsetCenterY, float speedEffect, MovingPartsType movingPartsType, float size)
    {
        public string TextureName { get; set; } = textureName;
        public float SpeedEffect {  get; set; } = speedEffect;
        public MovingPartsType MovingPartsType { get; set; } = movingPartsType;
        public float Rotation { get; set; } = 0;
        public float OffsetX { get; set; } = offsetCenterX;
        public float OffsetY { get; set; } = offsetCenterY;
        public float Size { get; set; } = size;
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

    public class Equipment : Element
    {
        public Equipment(float offsetCenterX, float offsetCenterY, float size, string name, float X, float Y, string textureFolderPath, Dictionary<string, Gun> guns, Dictionary<string, MovingParts> movingParts, float maxSpeed, float acceleration, float friction, float maxSpeedRotation, float maxHealth, EquipmentMoveType moveType, int maxHeightToBeOvercome, int maxDepthToBeOvercome) : base(name, textureFolderPath)
        {
            Health = maxHealth;
            WorldX = X;
            WorldY = Y;
            Guns = guns;
            MovingParts = movingParts;
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Friction = friction;
            MaxSpeedRotation = maxSpeedRotation;
            MoveType = moveType;
            OffsetX = offsetCenterX;
            OffsetY = offsetCenterY;
            Size = size;
            MaxHeightToBeOvercome = maxHeightToBeOvercome;
            MaxDepthToBeOvercome = maxDepthToBeOvercome;
        }

        public Equipment(float offsetCenterX, float offsetCenterY, float size, string name, float X, float Y, string textureFolderPath, Dictionary<string, Gun> guns, Dictionary<string, MovingParts> movingParts, float maxSpeed, float acceleration, float maxSpeedRotation, float maxHealth, EquipmentMoveType moveType) : base(name, textureFolderPath)
        {
            Health = maxHealth;
            WorldX = X;
            WorldY = Y;
            Guns = guns;
            MovingParts = movingParts;
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Friction = acceleration;
            MaxSpeedRotation = maxSpeedRotation;
            MoveType = moveType;
            OffsetX = offsetCenterX;
            OffsetY = offsetCenterY;
            Size = size;
            MaxHeightToBeOvercome = -1;
            MaxDepthToBeOvercome = -1;
        }

        public float Health { get; set; }  // Текущее здоровье
        public float WorldX { get; set; }  // Позиция по X относительно мира
        public float WorldY { get; set; }  // Позиция по Y относительно мира
        public Dictionary<string, Gun> Guns { get; set; }  // Список орудий
        public Dictionary<string, MovingParts> MovingParts { get; set; }  // Список постоянных эффектов
        public float MaxSpeed { get; set; }  // Максимальная скорость передвижения
        public float Rotation { get; set; } = 0; // Текущий поворот корпуса
        public float MaxSpeedRotation { get; set; }  // Максимальная скорость поворота корпуса
        public Vector2 Velocity { get; set; } = Vector2.Zero; // Текущая скорость
        public float Acceleration { get; set; }  // Ускорение
        public float Friction { get; set; }      // Замедление
        public float Size { get; set; }   // Размер ( в блоках )
        public float OffsetX { get; set; }  // Смещение центра по X относительно текстуры
        public float OffsetY { get; set; }  // Смещение центра по Y относительно текстуры
        public EquipmentMoveType MoveType { get; set; }  // Тип передвижения
        public float WorldPointX { get; set; } = -1;  // Точка X для перемещения в неё
        public float WorldPointY { get; set; } = -1;  // Точка Y для перемещения в неё

        public int MaxHeightToBeOvercome { get; set; } // Максимальная проходимая высота
        public int MaxDepthToBeOvercome { get; set; } // Максимальная проходимая глубина

        public float Height { get; set; } = 3; // Высота полёта ( для воздушных )

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
        public void InputHalderEquipment(KeyboardState key ,float dt)
        {
            // Направляющий вектор ( (0,0) если нет ввода )
            Vector2 move = Vector2.Zero;
            if (key.IsKeyDown(Keys.W)) move.Y -= 1;
            if (key.IsKeyDown(Keys.S)) move.Y += 1;
            if (key.IsKeyDown(Keys.A)) move.X -= 1;
            if (key.IsKeyDown(Keys.D)) move.X += 1;

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

            MatrixCamera.WorldPosX = WorldX;
            MatrixCamera.WorldPosY = WorldY;
        }
        public void Update(float dt)
        {
            if (MovingParts != null) // Обновление эффектов
            {
                var unitEffectList = MovingParts.Keys;
                foreach (var effect in unitEffectList) MovingParts[effect].MovingPartsUpdate(dt);
            }

            if (Velocity != Vector2.Zero) // Обновление движения
            {
                WorldX += Velocity.X * dt;
                WorldY += Velocity.Y * dt;
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
}
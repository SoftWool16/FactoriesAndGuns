using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    public class Element(string name, string texturePath)
    {
        public string Name { get; set; } = name;
        public string TexturePath { get; set; } = texturePath;
    }

    public enum EffectType
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

    public class Effect (string textureName, float offsetCenterX, float offsetCenterY, float speedEffect, EffectType effectType, float size)
    {
        public string TextureName { get; set; } = textureName;
        public float SpeedEffect {  get; set; } = speedEffect;
        public EffectType EffectType { get; set; } = effectType;
        public float Rotation { get; set; } = 0;
        public float OffsetX { get; set; } = offsetCenterX;
        public float OffsetY { get; set; } = offsetCenterY;
        public float Size { get; set; } = size;
        public void EffectUpdate(float dt)
        {
            if (EffectType == EffectType.rotation) Rotation += SpeedEffect * dt;
            //else if (EffectType == EffectType.shaking) 
        }
    }

    public class BaseEquipment(float offsetCenterX, float offsetCenterY, float size, string name, float X, float Y, string textureBodyPath, Dictionary<string, Gun> Guns, Dictionary<string, Effect> constantEffects, float maxSpeed, float acceleration, float friction, float maxSpeedRotation, float maxHealth, EquipmentMoveType moveType) : Element(name, textureBodyPath)
    {
        public float Health { get; set; } = maxHealth; // Текущее здоровье
        public float WorldX { get; set; } = X; // Позиция по X относительно мира
        public float WorldY { get; set; } = Y; // Позиция по Y относительно мира
        public Dictionary<string, Gun> Guns { get; set; } = Guns; // Список орудий
        public Dictionary<string, Effect> Effects { get; set; } = constantEffects; // Список постоянных эффектов
        public float MaxSpeed { get; set; } = maxSpeed; // Максимальная скорость передвижения
        public float Rotation { get; set; } = 0; // Текущий поворот корпуса
        public float MaxSpeedRotation { get; set; } = maxSpeedRotation; // Максимальная скорость поворота корпуса
        public Vector2 Velocity { get; set; } = Vector2.Zero; // текущая скорость
        public float Acceleration { get; set; } = acceleration; // ускорение
        public float Friction { get; set; } = friction;     // замедление, когда нет ввода
        public float Size { get; set; } = size;  // Размер ( в блоках )
        public float OffsetX { get; set; } = offsetCenterX; // Смещение центра по X относительно текстуры
        public float OffsetY { get; set; } = offsetCenterY; // Смещение центра по Y относительно текстуры
        public EquipmentMoveType MoveType { get; set; } = moveType; // Тип передвижения
        public float WorldPointX { get; set; } = X; // Точка X для перемещения в неё
        public float WorldPointY { get; set; } = Y; // Точка Y для перемещения в неё

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
    }

    public class GroundEquipment(float offsetCenterX, float offsetCenterY, float size,
        string name, float X, float Y, string textureBodyPath, Dictionary<string, Gun> Guns, int max_height_to_be_overcome,
        Dictionary<string, Effect> constantEffects, float maxSpeed, float acceleration, float friction, float maxSpeedRotation, string supportTextureName, float maxHealth, EquipmentMoveType moveType
        ) : BaseEquipment(offsetCenterX, offsetCenterY, size, name, X, Y, textureBodyPath, Guns, constantEffects, maxSpeed, acceleration, friction, maxSpeedRotation, maxHealth, moveType)
    {
        public int MaxHeightToBeOvercome { get; set; } = max_height_to_be_overcome;
        public string SupportTextureName { get; set; } = supportTextureName;
    }

    public class AirEquipment(float offsetCenterX, float offsetCenterY, float size,
        string name, float X, float Y, string textureBodyName, Dictionary<string, Gun> Guns,
        Dictionary<string, Effect> constantEffects, float maxSpeed, float accelerationAndFriction, float maxSpeedRotation, string shadowTextureName, float maxHealth
        ) : BaseEquipment(offsetCenterX, offsetCenterY, size, name, X, Y, textureBodyName, Guns, constantEffects, maxSpeed, accelerationAndFriction, accelerationAndFriction, maxSpeedRotation, maxHealth, EquipmentMoveType.hovering)
    {
        public string ShadowTextureName { get; set; } = shadowTextureName;
        public float Height { get; set; } = 3;
    }

    public class BaseFactory(string Name, string bodyTextureName, int size, Effect constantEffect, Effect dynamicEffect, Dictionary<string, Element> elementsIn, Dictionary<string, Element> elementsOut, float maxHealth, int capacity) : Element(Name, bodyTextureName)
    {
        public int Capacity { get; set; } = capacity;
        public float Health { get; set; } = maxHealth;
        public int Size { get; set; } = size;
        public Effect ConstantEffect { get; set; } = constantEffect;
        public Effect DynamicEffect { get; set; } = dynamicEffect;
        public Dictionary<string, Element> ElementsIn { get; set; } = elementsIn;
        public Dictionary <string, Element> ElementsOut { get; set; } = elementsOut;
        public Dictionary<string, Element> Elements { get; set; } = null;
    }
}
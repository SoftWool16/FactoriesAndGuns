using Microsoft.Xna.Framework;
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

    public class Bullet(string textureName, float size)
    {
        public string TextureName { get; set; } = textureName;
        public float Rotation { get; set; } = 0;
        public float WorldX { get; set; } = 0;
        public float WorldY { get; set; } = 0;
        public float Size { get; set; } = size;
        public void BulletUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
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
        public void EffectUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (EffectType == EffectType.rotation) Rotation += SpeedEffect * dt;
            //else if (EffectType == EffectType.shaking) 
        }
    }

    public class BaseEquipment(float offsetCenterX, float offsetCenterY, float size, string name, float X, float Y, string textureBodyPath, Dictionary<string, Gun> Guns, Dictionary<string, Effect> constantEffects, float maxSpeed, float maxSpeedRotation, float maxHealth) : Element(name, textureBodyPath)
    {
        public float Health { get; set; } = maxHealth;
        public float WorldX { get; set; } = X;
        public float WorldY { get; set; } = Y;
        public Dictionary<string, Gun> Guns { get; set; } = Guns;
        public Dictionary<string, Effect> Effects { get; set; } = constantEffects;
        public float MaxSpeed { get; set; } = maxSpeed;
        public float Rotation { get; set; } = 0;
        public float MaxSpeedRotation { get; set; } = maxSpeedRotation;
        public float Size { get; set; } = size;
        public float OffsetX { get; set; } = offsetCenterX;
        public float OffsetY { get; set; } = offsetCenterY;
        public void SmoothRotation(Vector2 vector2, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float rotation = Rotation;
            float rotationSpeed = MaxSpeedRotation;
            // Зависит от того, куда смотрит "нос" вашего спрайта по умолчанию.
            // Если вверх: MathF.Atan2(move.X, -move.Y)
            // Если вправо: MathF.Atan2(move.Y, move.X)
            float atan = MathF.Atan2(vector2.X, -vector2.Y);

            // Кратчайшее отклонение
            float angleDiff = MathHelper.WrapAngle(atan - Rotation);

            // Ограничиваем шаг за кадр
            float maxStep = rotationSpeed * dt;
            float step = MathHelper.Clamp(angleDiff, -maxStep, maxStep);

            //Обновляем угол и снова «заворачиваем», чтобы он оставался в [-p,p] ( p - число пи )
            Rotation = MathHelper.WrapAngle(Rotation + step);
        }
    }

    public class GroundEquipment(float offsetCenterX, float offsetCenterY, float size,
        string name, float X, float Y, string textureBodyPath, Dictionary<string, Gun> Guns, int max_height_to_be_overcome,
        Dictionary<string, Effect> constantEffects, float maxSpeed, float maxSpeedRotation, string supportTextureName, float maxHealth
        ) : BaseEquipment(offsetCenterX, offsetCenterY, size, name, X, Y, textureBodyPath, Guns, constantEffects, maxSpeed, maxSpeedRotation, maxHealth)
    {
        public int MaxHeightToBeOvercome { get; set; } = max_height_to_be_overcome;
        public string SupportTextureName { get; set; } = supportTextureName;
    }

    public class AirEquipment(float offsetCenterX, float offsetCenterY, float size,
        string name, float X, float Y, string textureBodyName, Dictionary<string, Gun> Guns,
        Dictionary<string, Effect> constantEffects, float maxSpeed, float maxSpeedRotation, string shadowTextureName, float maxHealth
        ) : BaseEquipment(offsetCenterX, offsetCenterY, size, name, X, Y, textureBodyName, Guns, constantEffects, maxSpeed, maxSpeedRotation, maxHealth)
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
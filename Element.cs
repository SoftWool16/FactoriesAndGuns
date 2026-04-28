using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Element(string name, double X, double Y, string texturePath)
    {
        public string Name { get; set; } = name;
        public string TexturePath { get; set; } = texturePath;
        public double WorldX { get; set; } = X;
        public double WorldY { get; set; } = Y;
    }

    public enum EffectType
    {
        rotation,
        shaking,
        movement
    }

    public struct TextureGun(string textureName, double offsetCenterX, double offsetCenterY)
    {
        public string TextureName { get; set; } = textureName;
        public double Rotation { get; set; } = 0;
        public double OffsetX { get; set; } = offsetCenterX;
        public double OffsetY { get; set; } = offsetCenterY;
    }

    public struct TextureEffect (string textureName, double offsetCenterX, double offsetCenterY, double speedEffect, EffectType effectType)
    {
        public string TextureName { get; set; } = textureName;
        public double SpeedEffect {  get; set; } = speedEffect;
        public EffectType EffectType { get; set; } = effectType;
        public double Rotation { get; set; } = 0;
        public double OffsetX { get; set; } = offsetCenterX;
        public double OffsetY { get; set; } = offsetCenterY;
    }

    internal class BaseEquipment(double offsetCenterX, double offsetCenterY, double sizeX, double sizeY, string name, double X, double Y, string textureBodyPath, Dictionary<string, TextureGun> Guns, Dictionary<string, TextureEffect> constantEffects, double maxSpeed, int max_height_to_be_overcome) : Element(name, X, Y, textureBodyPath)
    {
        public Dictionary<string, TextureGun> Guns { get; set; } = Guns;
        public Dictionary<string, TextureEffect> Effects { get; set; } = constantEffects;
        public double MaxSpeed { get; set; } = maxSpeed;
        public int MaxHeightToBeOvercome { get; set; } = max_height_to_be_overcome;
        public double Rotation { get; set; } = 0;
        public double FixedSizeX { get; set; } = sizeX;
        public double FixedSizeY { get; set; } = sizeY;
        public double OffsetX { get; set; } = offsetCenterX;
        public double OffsetY { get; set; } = offsetCenterY;
    }

    internal class GroundEquipment(double offsetCenterX, double offsetCenterY, double sizeX, double sizeY, string name, double X, double Y, string textureBodyPath, Dictionary<string, TextureGun> Guns, int max_height_to_be_overcome, Dictionary<string, TextureEffect> constantEffects, double maxSpeed) : BaseEquipment(offsetCenterX, offsetCenterY, sizeX, sizeY, name, X, Y, textureBodyPath, Guns, constantEffects, maxSpeed, max_height_to_be_overcome)
    {

    }

    internal class AirEquipment(double offsetCenterX, double offsetCenterY, double sizeX, double sizeY, string name, double X, double Y, string textureBodyName, Dictionary<string, TextureGun> Guns, Dictionary<string, TextureEffect> constantEffects, double maxSpeed) : BaseEquipment(offsetCenterX, offsetCenterY, sizeX, sizeY, name, X, Y, textureBodyName, Guns, constantEffects, maxSpeed, 5)
    {

    }
}
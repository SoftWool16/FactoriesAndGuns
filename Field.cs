using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; } = "New Field";

        public Element[,] FieldBackground = null;                              // Фон
        public BaseFactory[,] FieldBuild = null;                               // Постройки
        public Dictionary<string, Dictionary<string, Equipment>> FieldEquipment = []; // Техника
        public Bullet[] BulletList = [];                                // Снаряды
        public int SizeX { get; set; } = 0;
        public int SizeY { get; set; } = 0;
        public Equipment CurrentEquipment { get; set; } = null;

        //public enum TypeEffectsOnField
        //{
        //    hit,
        //    explosion,
        //    vortex,
        //    slipping
        //}

        //public Field (string name, Element[,] fields, int sizeX, int sizeY)
        //{
        //    Name = name;
        //    FieldBackground = fields;
        //    SizeX = sizeX;
        //    SizeY = sizeY;
        //}
        public Field (string name, int sizeX, int sizeY)
        {
            Name = name;
            FieldBackground = new Element[sizeY, sizeX];
            FieldBuild = new BaseFactory[sizeY, sizeX];
            SizeX = sizeX;
            SizeY = sizeY;

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (
                        i == j ||
                        i == j + 1 ||
                        j == i + 1
                        ) FieldBackground[i, j] = new Element("sand", "Block");
                    else if (i % 10 == 0 && j % 10 == 0) FieldBackground[i, j] = new Element("water", "Block");
                    else FieldBackground[i, j] = new Element("grass", "Block");
                }
            }

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    FieldBuild[i, j] = null;
                }
            }

            FieldEquipment["ground"] = [];
            FieldEquipment["air"] = [];

            MovingParts effects1 = new("drill", 0, 0, 2, MovingPartsType.rotation, 2);
            Dictionary<string, Element> elementsOut = [];
            elementsOut["metal"] = new("metal1", "metal");
            FieldBuild[8, 8] = new("Elementary_drill", "Build/Elementary_drill", 2, effects1, null, null, elementsOut, 100, 10);

            //Effect effects2 = new("conveyor", 0, 0, 5, EffectType.rotation, 1);
            //FieldBuild[10, 8] = new("conveyor1", "conveyor", 1, effects2, null, null, null, 100, 10);

            Dictionary<string, Gun> tower = [];
            Bullet bullet = new("bullet1", 0.1f);
            tower["tower1"] = new Gun("tower", 0, -1.3f, 100, bullet, 0.1f, 3, 4, 1.2f);
            FieldEquipment["ground"]["beta1"] = new Equipment(1, 1, 1.8f, "Beta", 1.5f, 1.5f, "Ground_Equipment/Beta", tower, null, 20, 20, 50, 6, 1000, EquipmentMoveType.tracked, 0, 1);

            Dictionary<string, MovingParts> effects = []; // Создание списка с подвижными частями
            effects["effect1"] = new MovingParts("propeller", 0, 0, 15, MovingPartsType.rotation, 4.6f);
            FieldEquipment["air"]["dragonfly1"] = new Equipment(1, 0.6f, 4.6f, "Dragonfly", 5.5f, 5.5f, "Air_Equipment/Dragonfly", null, effects, 25, 40, 5, 300, EquipmentMoveType.hovering);

            CurrentEquipment = FieldEquipment["air"]["dragonfly1"];
        }
        public void Update(GameTime gameTime)
        {
            var key = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentEquipment?.InputHalderEquipment(key, dt);

            if (key.IsKeyDown(Keys.D1))
            {
                if (CurrentEquipment != FieldEquipment["air"]["dragonfly1"]) CurrentEquipment = FieldEquipment["air"]["dragonfly1"];
            }

            if (key.IsKeyDown(Keys.D2))
            {
                if (CurrentEquipment != FieldEquipment["ground"]["beta1"]) CurrentEquipment = FieldEquipment["ground"]["beta1"];
            }

            if (key.IsKeyDown(Keys.Up) && MatrixCamera.SizeY < 150)
            {
                MatrixCamera.SizeY *= 1.1f;
                MatrixCamera.SizeX *= 1.1f;
            }
            if (key.IsKeyDown(Keys.Down) && MatrixCamera.SizeY > 30)
            {
                MatrixCamera.SizeY /= 1.1f;
                MatrixCamera.SizeX /= 1.1f;
            }

            var unitTypeList = FieldEquipment.Keys;
            foreach (string type in unitTypeList)
            {
                var unitList = FieldEquipment[type].Keys;
                foreach (string name in unitList)
                {
                    var unit = FieldEquipment[type][name];

                    var effects = unit.MovingParts;
                    if (effects != null) // Обновление эффектов
                    {
                        var unitEffectList = effects.Keys;
                        foreach (var effect in unitEffectList) effects[effect].MovingPartsUpdate(dt);
                    }

                    if (unit.Velocity != Vector2.Zero) // Обновление движения
                    {
                        FieldEquipment[type][name].WorldX += unit.Velocity.X * dt;
                        FieldEquipment[type][name].WorldY += unit.Velocity.Y * dt;
                    }
                }
            }

            BaseFactory[,] buildList = FieldBuild;
            if (buildList != null)
            {
                for (int i = 0; i < SizeY; i++)
                {
                    for (int j = 0; j < SizeX; j++)
                    {
                        buildList[i, j]?.MovingParts?.MovingPartsUpdate(dt);
                    }
                }
            }
        }
        //public void AddEffect(float x,  float y, float time, float size, TypeEffectsOnField type)
        //{
        //
        //}
    }
}
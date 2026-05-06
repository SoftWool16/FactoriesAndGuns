using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }

        public Element[,] FieldBackground;                              // Слой 1 - фоновый.
        public BaseFactory[,] FieldBuild;                               // Слой 2 - постройки.
        public Dictionary<string, GroundEquipment> FieldEquipment = []; // Слой 3 - прочая техника.
        public Bullet[] BulletList = [];                                // Слой 4 - пули, бомбы и т.п.
        public Dictionary<string, AirEquipment> AirEquipment = [];      // Слой 5 - воздушная техника.
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public GroundEquipment CurrentEquipment { get; set; } = null;
        public AirEquipment CurrentAirEquipment { get; set; } = null;

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

            Effect effects1 = new("drill", 0, 0, 2, EffectType.rotation, 2);
            Dictionary<string, Element> elementsOut = [];
            elementsOut["metal"] = new("metal1", "metal");
            FieldBuild[8, 8] = new("Elementary_drill", "Build/Elementary_drill", 2, effects1, null, null, elementsOut, 100, 10);

            //Effect effects2 = new("conveyor", 0, 0, 5, EffectType.rotation, 1);
            //FieldBuild[10, 8] = new("conveyor1", "conveyor", 1, effects2, null, null, null, 100, 10);

            Dictionary<string, Gun> tower = [];
            Bullet bullet = new("bullet1", 0.1f);
            tower["tower1"] = new Gun("tower", 0, -1.6f, 100, bullet, 0.1f, 3, 4, 1);
            FieldEquipment["beta1"] = new GroundEquipment(1, 1, 1.5f, "Beta", 1.5f, 1.5f, "Ground_Equipment/Beta", tower, 1, null, 10, null, 1000);

            Dictionary<string, Effect> effects = []; // Создание списка с эффектами
            effects["effect1"] = new Effect("propeller", 0, 0, 15, EffectType.rotation, 3);
            AirEquipment["dragonfly1"] = new AirEquipment(1, 0.6f, 3, "Dragonfly", 5.5f, 5.5f, "Air_Equipment/Dragonfly", null, effects, 5, null, 300);

            CurrentAirEquipment = AirEquipment["dragonfly1"];
        }
        public void InputHalder(GameTime gameTime)
        {
            var key = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //float WorldY1 = CurrentAirEquipment.WorldY;
            //float WorldX1 = CurrentAirEquipment.WorldX;

            //float MaxSpeed1 = CurrentAirEquipment.MaxSpeed;

            //float WorldX2 = CurrentAirEquipment.WorldX - 7;

            if (CurrentAirEquipment != null)
            {
                if (key.IsKeyDown(Keys.W)) CurrentAirEquipment.WorldY -= CurrentAirEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.S)) CurrentAirEquipment.WorldY += CurrentAirEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.A)) CurrentAirEquipment.WorldX -= CurrentAirEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.D)) CurrentAirEquipment.WorldX += CurrentAirEquipment.MaxSpeed * dt;

                MatrixCamera.WorldPosX = CurrentAirEquipment.WorldX;
                MatrixCamera.WorldPosY = CurrentAirEquipment.WorldY;
            }

            if (CurrentEquipment != null)
            {
                if (key.IsKeyDown(Keys.W)) CurrentEquipment.WorldY -= CurrentEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.S)) CurrentEquipment.WorldY += CurrentEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.A)) CurrentEquipment.WorldX -= CurrentEquipment.MaxSpeed * dt;
                if (key.IsKeyDown(Keys.D)) CurrentEquipment.WorldX += CurrentEquipment.MaxSpeed * dt;

                MatrixCamera.WorldPosX = CurrentEquipment.WorldX;
                MatrixCamera.WorldPosY = CurrentEquipment.WorldY;
            }

            if (key.IsKeyDown(Keys.Up) && MatrixCamera.SizeY < 60)
            {
                MatrixCamera.SizeY *= 1.1f;
                MatrixCamera.SizeX *= 1.1f;
            }
            if (key.IsKeyDown(Keys.Down) && MatrixCamera.SizeY > 10)
            {
                MatrixCamera.SizeY /= 1.1f;
                MatrixCamera.SizeX /= 1.1f;
            }

            var unitList = FieldEquipment.Keys;
            foreach (string name in unitList) // Обновление эффектов
            {
                var effects = FieldEquipment[name].Effects;
                if (effects != null)
                {
                    var unitEffectList = effects.Keys;
                    foreach (var effect in unitEffectList) effects[effect].EffectUpdate(gameTime);
                }
            }

            var airUnitList = AirEquipment.Keys;
            foreach (string name in airUnitList) // Обновление эффектов
            {
                var effects = AirEquipment[name].Effects;
                if (effects != null)
                {
                    var unitEffectList = effects.Keys;
                    foreach (var effect in unitEffectList) effects[effect].EffectUpdate(gameTime);
                }
            }

            BaseFactory[,] buildList = FieldBuild;
            if (buildList != null)
            {
                for (int i = 0; i < SizeY; i++)
                {
                    for (int j = 0; j < SizeX; j++)
                    {
                        buildList[i, j]?.ConstantEffect?.EffectUpdate(gameTime);
                    }
                }
            }
        }
    }
}
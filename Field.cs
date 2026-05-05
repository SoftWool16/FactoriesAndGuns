using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }

        public Element[,] FieldBackground;                      // Слой 1 - фоновый.
        public BaseFactory[,] FieldBuild;                           // Слой 2 - постройки.
        public Dictionary<string, AirEquipment> AirEquipment = [];    // Слой 3 - воздушная техника.
        public Dictionary<string, GroundEquipment> FieldEquipment = [];    // Слой 4 - прочая техника.
        public int SizeX { get; set; }
        public int SizeY { get; set; }

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
            tower["tower1"] = new Gun("tower", 0, -1.5f, 100, bullet, 0.1f, 3, 3, 0.6f);
            FieldEquipment["beta1"] = new GroundEquipment(1, 1, 1, 1, "Beta", 1.5f, 1.5f, "Ground_Equipment/Beta", tower, 1, null, 2.5f, null, 1000);

            Dictionary<string, Effect> effects = []; // Создание списка с эффектами
            effects["effect1"] = new Effect("propeller", 0, 0, 15, EffectType.rotation, 2.7f);
            AirEquipment["dragonfly1"] = new AirEquipment(1, 0.6f, 3, 3, "Dragonfly", 5.5f, 5.5f, "Air_Equipment/Dragonfly", null, effects, 1, null, 300);
        }
    }
}
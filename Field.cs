using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }

        public Element[,] FieldBackground;                      // Слой 1 - фоновый.
        //public Element[,] FieldBuild;                           // Слой 2 - постройки.
        public Dictionary<string, BaseEquipment> FieldEquipment = [];    // Слой 3 - техника.
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
            //FieldBuild = new Element[sizeY, sizeX];
            SizeX = sizeX;
            SizeY = sizeY;

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (i == j) FieldBackground[i, j] = new Element("point", j, i, "Block");
                    else if (i % 10 == 0 && j % 10 == 0) FieldBackground[i, j] = new Element("grass", j, i, "Block");
                    else FieldBackground[i, j] = new Element("void", j, i, "Block");
                }
            }

            Dictionary<string, TextureGun> tower = []; 
            tower["tower1"] = new TextureGun("tower", 0, 0);
            FieldEquipment["beta1"] = new GroundEquipment(1, 1, 1, 1, "Beta", 1.5, 1.5, "Ground_Equipment/Beta", tower, 1, null, 2.5);

            Dictionary<string, TextureEffect> effects = []; // Создание списка с эффектами
            effects["effect1"] = new TextureEffect("propeller", 0, 0, 8.5, EffectType.rotation);
            FieldEquipment["dragonfly1"] = new GroundEquipment(1, 0.6, 2, 2, "Dragonfly", 5.5, 5.5, "Air_Equipment/Dragonfly", null, 5, effects, 1);
        }
    }
}
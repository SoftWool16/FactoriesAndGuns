using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }

        public Element[,] FieldBackground;                      // Слой 1 - фоновый.
        //public Element[,] FieldBuild;                           // Слой 2 - постройки.
        public Dictionary<string, Dictionary<string, Element>> FieldGround = [];    // Слой 3 - наземная техника.
        //public Dictionary<string, Dictionary<string, Element>> FieldAir = [];       // Слой 4 - воздушная техника.
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
                    if (i == j) FieldBackground[i, j] = new Element("Point", j, i, ContentMaster.Textures["Block"]["point"]);
                    else if (i % 10 == 0 && j % 10 == 0) FieldBackground[i, j] = new Element("Grass", j, i, ContentMaster.Textures["Block"]["grass"]);
                    else FieldBackground[i, j] = new Element("Void", j, i, ContentMaster.Textures["Block"]["void"]);
                }
            }
            FieldGround["Beta1"] = [];
            FieldGround["Beta1"]["body"] = new Element("TankBody", 5, 5, ContentMaster.Textures["Ground_Equipment/Beta"]["body"]);
            FieldGround["Beta1"]["tower"] = new Element("TankTower", 5, 5, ContentMaster.Textures["Ground_Equipment/Beta"]["tower"]);
        }
    }
}
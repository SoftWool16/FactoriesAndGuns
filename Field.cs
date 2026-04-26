using System.Collections.Generic;

namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }

        public Element[,] FieldBackground;                      // Слой 1 - фоновый.
        public Element[,] FieldBuild;                           // Слой 2 - постройки.
        public Dictionary<string, Element> FieldTankBody = [];  // Слой 3 - тело наземной техники.
        public Dictionary<string, Element> FieldTankGuns = [];  // Слой 4 - орудия наземной техники.
        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public Field (string name, Element[,] fields, int sizeX, int sizeY)
        {
            Name = name;
            FieldBackground = fields;
            SizeX = sizeX;
            SizeY = sizeY;
        }
        public Field (string name, int sizeX, int sizeY)
        {
            Name = name;
            FieldBackground = new Element[sizeY, sizeX];
            FieldBuild = new Element[sizeY, sizeX];
            SizeX = sizeX;
            SizeY = sizeY;

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (i == j) FieldBackground[i, j] = new Element("Point", i, j, ContentMaster.Textures["point"]);
                    else if (i % 10 == 0 && j % 10 == 0) FieldBackground[i, j] = new Element("Grass", i, j, ContentMaster.Textures["grass"]);
                    else FieldBackground[i, j] = new Element("Void", i, j, ContentMaster.Textures["void"]);
                }
            }

            FieldTankBody["Player1"] = new Element("TankBody", 5, 5, ContentMaster.Textures["body"]);
        }
    }
}
namespace Factories_And_Guns
{
    internal class Field
    {
        public string Name { get; set; }
        public Element[,] Fields;
        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public Field (string name, Element[,] fields, int sizeX, int sizeY)
        {
            Name = name;
            Fields = fields;
            SizeX = sizeX;
            SizeY = sizeY;
        }
        public Field (string name, int sizeX, int sizeY)
        {
            Name = name;
            Fields = new Element[sizeY, sizeX];
            SizeX = sizeX;
            SizeY = sizeY;

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (i == j) Fields[i, j] = new Element("Point", i, j, "C:\\Users\\TOP\\Desktop\\модельки для MyMindustry\\static_block\\point.png");
                    else if (i % 10 == 0 && j % 10 == 0) Fields[i, j] = new Element("Grass", i, j, "C:\\Users\\TOP\\Desktop\\модельки для MyMindustry\\static_block\\grass.png");
                    else Fields[i, j] = new Element("Void", i, j, "C:\\Users\\TOP\\Desktop\\модельки для MyMindustry\\static_block\\void.png");
                }
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Factories_And_Guns
{
    public enum Difficulty
    {
        Casual, Easy, Medium, Difficult, VeryDifficult, Hellish
    }
    class Field
    {
        public string Name { get; set; } = "New Field";

        public Difficulty Difficulty { get; set; } = Difficulty.Medium;

        public Element[,] FieldBackground = null;                              // Фон
        public BaseFactory[,] FieldBuild = null;                               // Постройки
        public Dictionary<string, Dictionary<string, Equipment>> FieldEquipment = []; // Техника
        public TypeBullet[] BulletList = [];                                // Снаряды
        public int SizeX { get; set; } = 0;
        public int SizeY { get; set; } = 0;
        public Equipment CurrentEquipment { get; set; } = null;
        public int MouseWheelValue { get; set; } = 0;
        public bool Pause { get; set; } = false;

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
        public Field(string name, int sizeX, int sizeY, Difficulty difficulty)
        {
            Name = name;

            Difficulty = difficulty;

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

            Interface.CurrentTemplate[0] = "surface";

            LearningSystem.System = new Object[20, 20];

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    LearningSystem.System[i, j] = null;
                }
            }

            LearningSystem.System[10, 10] = new(null, null, null, TypeLearning.opened);
            LearningSystem.System[9, 10] = new(null, null, null, TypeLearning.closed);
            LearningSystem.System[8, 10] = new(null, null, null, TypeLearning.locked);

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0;j < 20; j++)
                {
                    if (LearningSystem.System[i, j] != null)
                    {
                        string nameL = "";
                        var type = LearningSystem.System[i, j].TypeLearning;
                        if (type == TypeLearning.opened) nameL = "opened";
                        else if (type == TypeLearning.closed) nameL = "closed";
                        else if (type == TypeLearning.locked) nameL = "closed";
                        Interface.Templates["learning"][$"l{i + j}"] = new(70 * i, 70 * j, nameL, $"User_Interface/frame", 50, false, false);
                    }
                }
            }

            MovingParts effects1 = new(0, 0, 0, 0, 2, MovingPartsType.rotation, 2);
            Dictionary<string, Element> elementsOut = [];
            elementsOut["metal"] = new("metal1", "metal");
            FieldBuild[8, 8] = new("Elementary_drill", "Build/Elementary_drill", 2, effects1, null, null, elementsOut, 100, 10);

            //Effect effects2 = new("conveyor", 0, 0, 5, EffectType.rotation, 1);
            //FieldBuild[10, 8] = new("conveyor1", "conveyor", 1, effects2, null, null, null, 100, 10);

            Dictionary<string, Gun> tower = [];
            TypeBullet bullet = new(0.1f, 7);
            tower["tower1"] = new Gun(1, 1.6f, 0, 0, 100, bullet, 0.1f, 3, 4, 1.2f, 0.3f, 0.05f);
            FieldEquipment["ground"]["beta1"] = new Equipment(1, 1, 0, 0, 1.8f, "Beta", 1.5f, 1.5f, "Ground_Equipment/Beta", tower, null, 20, 20, 50, 6, 1000, EquipmentMoveType.tracked, 0, 1);

            Dictionary<string, MovingParts> effects = []; // Создание списка с подвижными частями
            effects["effect1"] = new MovingParts(1, 1, 0, 0, 15, MovingPartsType.rotation, 6);
            FieldEquipment["air"]["dragonfly1"] = new Equipment(1, 0.6f, 0, 0, 2f, "Dragonfly", 5.5f, 5.5f, "Air_Equipment/Dragonfly", null, effects, 25, 40, 5, 300, EquipmentMoveType.hovering);

            CurrentEquipment = FieldEquipment["air"]["dragonfly1"];

            //Interface.Templates["field"]["scope"] = new(0, 0, "base", "User_Interface/scopes", 30);
            //Interface.Templates["surface"]["top"] = new(0, 0, "background", "User_Interface", 500, 100);
        }
        public void Update(float dt, Keys[] keys, MouseState mouse)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == Keys.D1)
                {
                    if (CurrentEquipment != FieldEquipment["air"]["dragonfly1"]) CurrentEquipment = FieldEquipment["air"]["dragonfly1"];
                }

                else if (keys[i] == Keys.D2)
                {
                    if (CurrentEquipment != FieldEquipment["ground"]["beta1"]) CurrentEquipment = FieldEquipment["ground"]["beta1"];
                }

                if (keys[i] == Keys.P) Pause = !Pause;
            }

            if (mouse.ScrollWheelValue < MouseWheelValue && MatrixCamera.SizeX < 350)
            {
                MatrixCamera.SizeVector = 1.05f;
            }
            else if (mouse.ScrollWheelValue > MouseWheelValue && MatrixCamera.SizeX > 60)
            {
                MatrixCamera.SizeVector = 0.95f;
            }
            MouseWheelValue = mouse.ScrollWheelValue;

            MatrixCamera.CameraUpdate();

            if (Pause == false)
            {
                CurrentEquipment?.InputHalderEquipment(keys, mouse, dt);

                var unitTypeList = FieldEquipment.Keys;
                foreach (string type in unitTypeList)
                {
                    var unitList = FieldEquipment[type].Keys;
                    foreach (string name in unitList)
                    {
                        bool x = true;
                        bool y = true;
                        var unit = FieldEquipment[type][name];
                        if (unit.Type == "ground") // Если единица наземная - обработка столкновений
                        {
                            float vX = unit.Velocity.X;
                            float vY = unit.Velocity.Y;

                            float unitSizeX = unit.Size / 2;
                            float unitSizeY = unit.Size / 2;

                            if (vX < 0) unitSizeX = -unitSizeX;
                            if (vY < 0) unitSizeY = -unitSizeY;

                            float uX = unit.WorldX + vX * dt + unitSizeX;
                            float uY = unit.WorldY + vY * dt + unitSizeY;

                            if (uX > 0 && uX < SizeX)
                            {
                                if (FieldBuild[(int)uX,
                                    (int)unit.WorldY] != null) x = false;
                            }
                            else { x = false; }

                            if (uY > 0 && uY < SizeY)
                            {
                                if (FieldBuild[(int)unit.WorldX,
                                    (int)uY] != null) y = false;
                            }
                            else { y = false; }
                        }
                        if (CurrentEquipment != unit) unit.SmoothStop(dt);

                        Vector2 mousePos = Vector2.Zero;

                        if (CurrentEquipment == unit)
                        {
                            mousePos = new
                            (
                                mouse.X,
                                mouse.Y
                            );
                        }

                        unit.Update(dt, x, y, mousePos);
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
            else
            {
                float speed = MatrixCamera.CameraSpeed * MatrixCamera.SizeY;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == Keys.W) MatrixCamera.WorldPosY -= speed;
                    if (keys[i] == Keys.S) MatrixCamera.WorldPosY += speed;
                    if (keys[i] == Keys.D) MatrixCamera.WorldPosX += speed;
                    if (keys[i] == Keys.A) MatrixCamera.WorldPosX -= speed;
                }
            }
        }
        //public void AddEffect(float x,  float y, float time, float size, TypeEffectsOnField type)
        //{
        //
        //}
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Factories_And_Guns
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch SpriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            Window.AllowUserResizing = true;  // Разрешаем пользователю менять размер окна вручную
        }

        protected override void Initialize()
        {
            // НАЗНАЧЕНИЕ: Здесь настраиваются неграфические объекты (переменные, логика).
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            OpenSource.GraphicsDevice = GraphicsDevice;

            Interface.Templates["field"] = [];
            Interface.Templates["surface"] = [];
            Interface.Templates["map"] = [];
            Interface.Templates["learning"] = [];
            Interface.Templates["settings"] = [];

            Interface.Templates["Sun system"] = [];
            Interface.Templates["Planet fields"] = [];

            Interface.Templates["Sun system"]["1"] = new(500, 500, "у", "d", 500, false, "Planet fields");
            Interface.Templates["Planet fields"]["1"] = new(500, 500, "w", "f", 300, false, "");

            Interface.CurrentTemplate[0] = "Sun system";

            Interface.Templates["learning"]["background"] = new(0, 0, "background", "User_Interface", 1, true, null);

            Interface.Keys["learning"] = Keys.I;
            Interface.Keys["back"] = Keys.Back;
            Interface.Keys["home"] = Keys.Escape;
            //Interface.Keys["the main mouse button"] = 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // ИНФО: Чтобы добавить контент, надо на верхней панели окна Visual Studio открыть: Средства -> Командная строка -> PowerShell ( или просто командную строку проекта ( View -> Terminal - для VS Code )) и ввести:
            //       dotnet mgcb-editor Content/Content.mgcb      - оно откроет MGCB Editor, там нажимаешь пкм на Content ( он будет слева, в окне Project )
            //       и добавляешь существующий объект:  Add -> Existing file или Existing folder.
            //       ПРИМЕЧАНИЕ: Можно добавлять сразу несколько файлов.
            //       В диалоговом окне нажимаешь "скопировать файл с диска" ( если файлов много - ставь галочку "применить ко всем выделенным" ).
            //       Когда закончишь добавлять, нажми, на панели сверху, Build -> Build ( файлы применятся ). Теперь можно использовать их в коде, например:
            //       Texture2D Block_texture = Content.Load<Texture2D>("Block/point");

            // НАЗНАЧЕНИЕ: Использовать this.Content.Load< >( ) для загрузки контента в переменную и т.п.
            //             Примечание: такой метод, в отличие от OpenSource.OpenPng(), тратит НАМНОГО МЕНЬШЕ оперативной памяти,
            //                         потому что текстуры загружены заранее, и хранятся не в коде, а в Content.

            ContentMaster.LoadTexture(Content); // Загружаем текстуры.

            General.SunSystems["A New Beginning"] = new("A New Beginning");
            General.SunSystems["A New Beginning"].Planets["New planet"] = new(0, null, 100);
            General.SunSystems["A New Beginning"].Planets["New planet"].Fields["New field"] = new("", 100, 100, Difficulty.Medium);

            LearningSystem.System[10, 10] = new(null, null, null, TypeLearning.opened);
            LearningSystem.System[9, 10] = new(null, null, null, TypeLearning.closed);
            LearningSystem.System[8, 10] = new(null, null, null, TypeLearning.locked);

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (LearningSystem.System[i, j] != null)
                    {
                        string nameL = "";
                        var type = LearningSystem.System[i, j].TypeLearning;
                        if (type == TypeLearning.opened) nameL = "opened";
                        else if (type == TypeLearning.closed) nameL = "closed";
                        else if (type == TypeLearning.locked) nameL = "closed";
                        Interface.Templates["learning"][$"l{i + j}"] = new(70 * i, 70 * j, nameL, $"User_Interface/frame", 50, false, "");
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();

            // НАЗНАЧЕНИЕ: Внутренняя логика.

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            var mouse = Mouse.GetState();

            Interface.Cursor.X = mouse.X;
            Interface.Cursor.Y = mouse.Y;

            Interface.Update(keys, mouse);

            General.Update(dt, keys, mouse);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // НАЗНАЧЕНИЕ: Отрисовка.

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp); // PointClamp - без сглаживания текстур (по-пиксельная отрисовка).

            MatrixCamera.RenderMatrix(Window, SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

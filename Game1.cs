using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace Factories_And_Guns
{
    public class Game1 : Game
    {
        private Field Field;

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

            MatrixCamera.SpriteBatch = SpriteBatch;
            MatrixCamera.GameWindow = Window;

            Interface.Templates["field"] = [];
            Interface.Templates["surface"] = [];
            Interface.Templates["map"] = [];
            Interface.Templates["learning"] = [];

            Interface.Templates["learning"]["background"] = new(0, 0, "background", "User_Interface", Window.ClientBounds.Width, Window.ClientBounds.Height);

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

            Field = new("New field", 100, 100); // Создаём поле после того, как загрузим текстуры.
            MatrixCamera.Field = Field;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // НАЗНАЧЕНИЕ: Внутренняя логика.
            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            Field.Update(gameTime, keys);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // НАЗНАЧЕНИЕ: Отрисовка.

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp); // PointClamp - без сглаживания текстур (по-пиксельная отрисовка).

            MatrixCamera.RenderMatrix();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

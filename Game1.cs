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
        private float CameraSpeed = 10f;
        private Field Field;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch SpriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;  // Разрешаем пользователю менять размер окна вручную
        }

        protected override void Initialize()
        {
            // НАЗНАЧЕНИЕ: Здесь настраиваются неграфические объекты (переменные, логика).
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            OpenSource.GraphicsDevice = GraphicsDevice;

            MatrixCamera.SpriteBatch = SpriteBatch;
            MatrixCamera.GameWindow = Window;

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

            var key = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (key.IsKeyDown(Keys.W)) MatrixCamera.WorldPosY -= CameraSpeed * dt;
            if (key.IsKeyDown(Keys.S)) MatrixCamera.WorldPosY += CameraSpeed * dt;
            if (key.IsKeyDown(Keys.A)) MatrixCamera.WorldPosX -= CameraSpeed * dt;
            if (key.IsKeyDown(Keys.D)) MatrixCamera.WorldPosX += CameraSpeed * dt;

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

            Field.FieldEquipment["beta1"].Rotation += CameraSpeed / 5 * dt;

            Field.FieldEquipment["dragonfly1"].Rotation += CameraSpeed / 5 * dt;

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

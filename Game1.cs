using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            Field = new("New field", 100, 100);
            Matrix.Field = Field;
            Matrix.SpriteBatch = SpriteBatch;
            Matrix.GameWindow = Window;

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

            Content.Load<Texture2D>("Block/point"); // Расширение не писать
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // НАЗНАЧЕНИЕ: Внутренняя логика

            var key = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (key.IsKeyDown(Keys.W)) Matrix.WorldPosY -= CameraSpeed * dt;
            if (key.IsKeyDown(Keys.S)) Matrix.WorldPosY += CameraSpeed * dt;
            if (key.IsKeyDown(Keys.A)) Matrix.WorldPosX -= CameraSpeed * dt;
            if (key.IsKeyDown(Keys.D)) Matrix.WorldPosX += CameraSpeed * dt;

            if (key.IsKeyDown(Keys.Up) && Matrix.SizeY < 60)
            {
                Matrix.SizeY *= 1.1f;
                Matrix.SizeX *= 1.1f;
            }
            if (key.IsKeyDown(Keys.Down) && Matrix.SizeY > 10)
            {
                Matrix.SizeY /= 1.1f;
                Matrix.SizeX /= 1.1f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // НАЗНАЧЕНИЕ: Отрисовка

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp); // PointClamp - без сглаживания текстур (по-пиксельная отрисовка)

            Matrix.RenderMatrix();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

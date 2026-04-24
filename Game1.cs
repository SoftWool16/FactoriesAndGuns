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

            // Разрешаем пользователю менять размер окна вручную
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Здесь настраиваются неграфические объекты (переменные, логика).
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


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Внутренняя логика

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

            // TODO: Отрисовка
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Matrix.RenderMatrix();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using WH_Physics;

namespace SweepCollisionDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private WH_Physics.WH_Physics _physics;
        private Random _random;
        private int _screenWidth;
        private int _screenHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth *= 2;
            _graphics.PreferredBackBufferHeight *= 2;
            _screenWidth = _graphics.PreferredBackBufferWidth;
            _screenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            _random = new Random(DateTime.Now.Millisecond);

            _physics = new WH_Physics.WH_Physics(_screenWidth, _screenHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D annoyedSakuraTexture = Content.Load<Texture2D>("annoyed_sakura");
            Texture2D happySakuraTexture = Content.Load<Texture2D>("happy_sakura");
            for (int i = 1; i <= 5; i++)
                _physics.addObject(_random.Next(0, _screenWidth - 100), _random.Next(0, _screenHeight - 100), _random.Next(100, 300), _random.Next(100, 300), (float)_random.NextDouble() * 2 - 1, (float)_random.NextDouble() * 2 - 1, annoyedSakuraTexture, happySakuraTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _physics.update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (DrawableObject obj in _physics.getDrawableObjectArray())
            {
                _spriteBatch.Draw(obj._texture, obj._rectangle, obj._color);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
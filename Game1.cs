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
            _screenWidth = _graphics.PreferredBackBufferWidth;
            _screenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            _random = new Random(DateTime.Now.Millisecond);

            _physics = new WH_Physics.WH_Physics();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D sakuraTexture = Content.Load<Texture2D>("annoyed_sakura");
            _physics.addObject(_random.Next(0, _screenWidth - 100), _random.Next(0, _screenHeight - 100), 100, 100, (float)_random.NextDouble(), (float)_random.NextDouble(), sakuraTexture);
            _physics.addObject(_random.Next(0, _screenWidth - 100), _random.Next(0, _screenHeight - 100), 100, 100, (float)_random.NextDouble(), (float)_random.NextDouble(), sakuraTexture);
            _physics.addObject(_random.Next(0, _screenWidth - 100), _random.Next(0, _screenHeight - 100), 100, 100, (float)_random.NextDouble(), (float)_random.NextDouble(), sakuraTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _physics.update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (DrawableObject obj in _physics.getDrawableObjectArray())
            {
                _spriteBatch.Draw(obj._texture, obj._rectangle, !obj._colliding ? Color.White : Color.Red);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
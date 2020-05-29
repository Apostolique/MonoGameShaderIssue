using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Apos.Input;

namespace GameProject {
    public class GameRoot : Game {
        public GameRoot() {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            _s = new SpriteBatch(GraphicsDevice);

            _image1 = Content.Load<Texture2D>("Image1");
            _image2 = Content.Load<Texture2D>("Image2");

            _mix = Content.Load<Effect>("Mix");

            InputHelper.Setup(this);
        }

        protected override void Update(GameTime gameTime) {
            InputHelper.UpdateSetup();

            if (_quit.Pressed())
                Exit();


            InputHelper.UpdateCleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _mix.Parameters["Image2"].SetValue(_image2);
            _s.Begin(effect: _mix);
            _s.Draw(_image1, Vector2.Zero, Color.White);
            _s.End();

            // This line breaks OpenGL.
            GetTexture(_s);

            base.Draw(gameTime);
        }

        Texture2D _image1;
        Texture2D _image2;

        Effect _mix;

        private static Texture2D _whitePixelTexture;

        private static Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_whitePixelTexture == null) {
                _whitePixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _whitePixelTexture.SetData(new[] { Color.White });
                spriteBatch.Disposing += (sender, args) => {
                    _whitePixelTexture?.Dispose();
                    _whitePixelTexture = null;
                };
            }

            return _whitePixelTexture;
        }

        GraphicsDeviceManager _graphics;
        SpriteBatch _s;

        ICondition _quit = new AnyCondition(
            new KeyboardCondition(Keys.Escape),
            new GamePadCondition(GamePadButton.Back, 0)
        );
    }
}
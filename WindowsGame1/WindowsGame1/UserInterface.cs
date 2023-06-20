using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Engine;

namespace WindowsGame1
{
    public class UserInterface : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private Camera2D _uiCamera;
        private MouseState _mouseState = Mouse.GetState();

        public UserInterface(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            DrawOrder = 2;
            _uiCamera = new Camera2D()
            {
                Position = new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2)
            };
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, _uiCamera.GetMatrix(Game.GraphicsDevice));

            Color hpColor = Color.Black;
            _spriteBatch.Draw(Game1.Textures["playerHUD"], new Vector2(10, 390), Color.White);
            for (int i = 0; i < Game1.Player.Health; i++)
            {
                hpColor = Color.Lerp(hpColor, new Color(255, 0, 0), 0.02f);
                _spriteBatch.Draw(Game1.Textures["uiHealth"], new Vector2(105 + i, 405), hpColor);
            }
            _spriteBatch.DrawString(Game1.SpriteFonts["hudfont"], Game1.Player.GetCurrentWeapon().AmmoCount.ToString(), new Vector2(100, 425), Color.White);

            #if DEBUG
                _spriteBatch.DrawString(Game1.SpriteFonts["vcr"], "Dev Build--", Vector2.Zero, Color.White);
            #endif
            #if PROTOTYPE
                _spriteBatch.DrawString(Game1.SpriteFonts["vcr"], "Pre Alpha Build--", Vector2.Zero, Color.White);
            #endif
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(Game1.Textures["uiAim"], new Vector2(_mouseState.X, _mouseState.Y), null, Color.White, 0, new Vector2(Game1.Textures["uiAim"].Width / 2, Game1.Textures["uiAim"].Height / 2), 1.0f, SpriteEffects.None, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

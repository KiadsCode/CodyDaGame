using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Weapons;

namespace WindowsGame1
{
    public class Player : DrawableGameComponent
    {
        public const string AssetsName = "codyGamePlay";

        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffect = SpriteEffects.None;
        private Vector2 _position = Vector2.Zero;
        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Weapon[] _weapons;
        private int _frame = 0;
        private int _health = 70;
        private int _weaponIndex = 0;
        private int _direction = 1;
        private bool _slided = false;

        public int Health
        {
            get
            {
                return _health;
            }
        }
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }
        public int Direction
        {
            get
            {
                return _direction;
            }
        }
        public Weapon[] Weapons
        {
            get
            {
                return _weapons;
            }
        }
        public Weapon GetCurrentWeapon()
        {
            return _weapons[_weaponIndex];
        }

        public Player(Game game)
            : base(game)
        {
            DrawOrder = 1;
            _weapons = new Weapon[] { new AssaultRifle(game), new Magnum(game) };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _weapons[_weaponIndex].ResetAnimation();
            RotateToDirection(1, SpriteEffects.FlipHorizontally);
            _frame = Game1.SpriteSheets[AssetsName].FramesCount - 3;
            base.LoadContent();
        }

        public Rectangle GetCollider2D()
        {
            return new Rectangle((int)_position.X - Game1.SpriteSheets[AssetsName][_frame].Width / 2, (int)_position.Y - Game1.SpriteSheets["codyGamePlay"][_frame].Height / 2, Game1.SpriteSheets["codyGamePlay"][_frame].Width, Game1.SpriteSheets["codyGamePlay"][_frame].Height);
        }

        private void RotateToDirection(int dir, SpriteEffects se)
        {
            _direction = dir;
            _frame = 1;
            _spriteEffect = se;
            _slided = false;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (_slided == false)
            {
                _frame += 2;
                _slided = _frame >= Game1.SpriteSheets[AssetsName].FramesCount - 1;
            }

            if (keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyUp(Keys.A))
                if (GetCollider2D().Right + 5 < 900)
                    _position.X += 5;
            if (keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyUp(Keys.D))
                if (GetCollider2D().Left - 5 > -100)
                    _position.X -= 5;

            RotatePlayer();
            if (keyboard.IsKeyDown(Keys.S))
                _position.Y += 5;
            if (keyboard.IsKeyDown(Keys.W))
                _position.Y -= 5;
            Game1.GamePlayCamera.Position = _position;

            bool switchingWeapon = keyboard.IsKeyDown(Keys.D1) && _oldKeyboardState.IsKeyUp(Keys.D1);
            if (switchingWeapon)
                SwitchWeapon();

            DebugFeatures();

            if (Game.IsActive == true)
            {
                foreach (Weapon item in _weapons)
                    item.Game = Game;
                _weapons[_weaponIndex].Update(this, gameTime);
            }

            _oldKeyboardState = keyboard;
            base.Update(gameTime);
        }

        private void RotatePlayer()
        {
            MouseState mouseState = Mouse.GetState();
            bool mouseAfterPlayer = Convert.ToSingle(mouseState.X) + (_position.X - Game.Window.ClientBounds.Width / 2) > _position.X;
            switch (mouseAfterPlayer)
            {
                case true:
                    if (_direction != 1)
                        RotateToDirection(1, SpriteEffects.FlipHorizontally);
                    break;
                case false:
                    if (_direction != -1)
                        RotateToDirection(-1, SpriteEffects.None);
                    break;
            }
        }

        private void DebugFeatures()
        {
#if DEBUG
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.N))
                _weapons[_weaponIndex].AddAmmo(1);
#endif
        }

        private void SwitchWeapon()
        {
            bool weaponIndeB = _weaponIndex != 1;
            _weapons[_weaponIndex].ResetAnimation();
            _weaponIndex = Convert.ToInt32(weaponIndeB);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Game1.GamePlayCamera.GetMatrix(Game.GraphicsDevice));

            _spriteBatch.Draw(Game1.Textures[AssetsName], _position, Game1.SpriteSheets[AssetsName][_frame], Color.White, 0, new Vector2(Game1.SpriteSheets[AssetsName][_frame].Width / 2, Game1.SpriteSheets[AssetsName][_frame].Height / 2), 1, _spriteEffect, 0);
            _weapons[_weaponIndex].Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

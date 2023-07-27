using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Engine.Collision;
using WindowsGame1.Engine.Map;
using WindowsGame1.Weapons;

namespace WindowsGame1
{
    public class Player : DrawableGameComponent, IHitable
    {
        public const string AssetsName = "codyGamePlay";
        public const int DashCooldown = 50;
        public const int DashPower = 34;

        private SpriteBatch _spriteBatch;

        private SpriteEffects _spriteEffect = SpriteEffects.None;
        private Vector2 _dashStrength = Vector2.Zero;
        private Vector2 _position = Vector2.Zero;
        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Weapon[] _weapons;
        private DashDirection _dashDirection = DashDirection.Right;
        private int _frame = 0;
        private int _health = 70;
        private int _weaponIndex = 0;
        private int _direction = 1;
        private int _dashCooldown = 25;
        private int _regenerationCooldown = 180;
        private bool _dashAvailable = false;
        private bool _slided = false;
        private bool _alive = true;

        public int Health
        {
            get
            {
                return _health;
            }
        }

        public bool Alive
        {
            get
            {
                return _alive;
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

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public Rectangle GetCollider()
        {
            int sizeSub = 5;
            return new Rectangle(
                (int)_position.X - ((Game1.SpriteSheets[AssetsName][_frame].Width / 2) - sizeSub),
                (int)_position.Y - ((Game1.SpriteSheets["codyGamePlay"][_frame].Height / 2) - sizeSub),
                Game1.SpriteSheets["codyGamePlay"][_frame].Width - (sizeSub * 2),
                Game1.SpriteSheets["codyGamePlay"][_frame].Height - (sizeSub * 2));
        }

        private void RotateToDirection(int dir, SpriteEffects se)
        {
            _direction = dir;
            _frame = 1;
            _spriteEffect = se;
            _slided = false;
        }

        private void ShakeCamera(float power)
        {
            if (Game1.CameraShakeAvailable)
                Game1.GamePlayCamera.Rotation = power;
        }

        private bool HasPotentialCollision(int xspeed = 0, int yspeed = 0)
        {
            Rectangle trueCollider = GetCollider();
            Rectangle potentialyCollider = new Rectangle(trueCollider.X + xspeed, trueCollider.Y + yspeed, trueCollider.Width, trueCollider.Height);
            bool hasPotentionalCollision = false;

            foreach (Block item in Game1.MapComponent.Blocks)
            {
                if (item.Type != 3)
                {
                    hasPotentionalCollision = item.GetCollider().Intersects(potentialyCollider);
                    if (hasPotentionalCollision)
                        break;
                }
            }

            return hasPotentionalCollision;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (_regenerationCooldown > 0)
                _regenerationCooldown--;
            if (_dashCooldown > 0)
                _dashCooldown--;
            _dashAvailable = _dashCooldown == 0;

            if (_regenerationCooldown == 0 && _health < 70)
                _health++;

            if (keyboard.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space))
            {
                if (_dashAvailable)
                {
                    _dashStrength = new Vector2(DashPower * (int)_dashDirection, 0);
                    ShakeCamera(0.15f * (float)_dashDirection);
                    Game1.SoundEffects["dashsound"].Play();
                    _dashCooldown = DashCooldown;
                    _regenerationCooldown = 180;
                }
            }

            _position += _dashStrength;
            Game1.GamePlayCamera.Rotation = MathHelper.Lerp(Game1.GamePlayCamera.Rotation, 0, 0.1f);
            _dashStrength = Vector2.Lerp(_dashStrength, Vector2.Zero, 0.1f);

            if (_slided == false)
            {
                _frame += 2;
                _slided = _frame >= Game1.SpriteSheets[AssetsName].FramesCount - 1;
            }

            Rectangle trueCollider = GetCollider();
            if (keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyUp(Keys.A))
            {
                if (HasPotentialCollision(5) == false)
                    _position.X += 5;
                _dashDirection = DashDirection.Right;
            }

            if (keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyUp(Keys.D))
            {
                if (HasPotentialCollision(-5) == false)
                    _position.X -= 5;
                _dashDirection = DashDirection.Left;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                if (HasPotentialCollision(0, 5) == false)
                    _position.Y += 5;
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                if (HasPotentialCollision(0, -5) == false)
                    _position.Y -= 5;
            }

            RotatePlayer();
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
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.R) && _oldKeyboardState.IsKeyUp(Keys.R))
                Game1.ResetEnemies();
            if (keyboard.IsKeyDown(Keys.N))
                _weapons[_weaponIndex].AddAmmo(1);
            if (keyboard.IsKeyDown(Keys.G) && _oldKeyboardState.IsKeyUp(Keys.G))
                Hit(15);
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
            if (_alive)
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Game1.GamePlayCamera.GetMatrix(Game.GraphicsDevice));

                _spriteBatch.Draw(Game1.Textures[AssetsName], _position, Game1.SpriteSheets[AssetsName][_frame], Color.White, 0, new Vector2(Game1.SpriteSheets[AssetsName][_frame].Width / 2, Game1.SpriteSheets[AssetsName][_frame].Height / 2), 1, _spriteEffect, 0);
                _weapons[_weaponIndex].Draw(_spriteBatch);
                
                //_spriteBatch.Draw(Game1.Textures["1x1"], GetCollider(), Color.Red);

                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void Hit(int damage)
        {
            _regenerationCooldown = 180;
            _health -= damage;
            _alive = _health > 0;
        }
    }
}

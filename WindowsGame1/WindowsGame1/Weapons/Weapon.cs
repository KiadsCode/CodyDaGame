using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.Weapons
{
    public class Weapon
    {
        private int _animationCoolDown = 2;
        private int _shootingCoolDown = 3;
        private SpriteEffects _spriteEffects = SpriteEffects.None;
        private Rectangle _bulletRectStart = Rectangle.Empty;
        private MouseState _oldMouseState = Mouse.GetState();

        protected string weaponAssetName = string.Empty;
        protected string weaponSpriteSheetName = string.Empty;
        protected int frame = 0;
        protected int shootingCoolDown = 3;
        protected int ammo = 0;
        protected bool automatical = false;
        protected bool ableToShoot = false;
        protected Cody player = default(Cody);
        protected Vector2 weaponOffsetRight = Vector2.Zero;
        protected Vector2 weaponOffsetLeft = Vector2.Zero;
        protected Vector2 shootPointOffsetRight = Vector2.Zero;
        protected Vector2 shootPointOffsetLeft = Vector2.Zero;
        protected ushort damage = 0;

        internal Game Game = null;

        public ushort Damage
        {
            get
            {
                return damage;
            }
        }
        public int ShootingCoolDown
        {
            get
            {
                return shootingCoolDown;
            }
        }
        public bool IsAbleToShoot
        {
            get
            {
                return ableToShoot;
            }
        }
        public int AmmoCount
        {
            get
            {
                return ammo;
            }
        }

        public Weapon(Game game)
        {
            // TODO: Construct any child components here
            Game = game;
            Initialize();
        }

        public virtual void Initialize()
        {
            // TODO: Add your initialization code here
            ResetAnimation();
        }

        public void ResetAnimation()
        { 
            frame = Game1.SpriteSheets[weaponSpriteSheetName].FramesCount - 2; 
        }

        public virtual void Update(Cody cody, GameTime gameTime)
        {
            // TODO: Add your update code here
            this.player = cody;
            MouseState mouseState = Mouse.GetState();
            if (_shootingCoolDown > 0)
                _shootingCoolDown--;
            TryShoot(mouseState);

            _animationCoolDown--;
            if (_animationCoolDown <= 0)
                if (frame <= Game1.SpriteSheets[weaponSpriteSheetName].FramesCount - 2)
                    frame++;
            switch (cody.Direction)
            {
                case 1:
                    _spriteEffects = SpriteEffects.None;
                    break;
                case -1:
                    _spriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                default:
                    break;
            }
            _oldMouseState = mouseState;
        }

        private void TryShoot(MouseState mouseState)
        {
            ableToShoot = _shootingCoolDown == 0;
            if (ableToShoot)
            {
                if (automatical)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        PreShoot();
                    }
                }
                else
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                    {
                        PreShoot();
                    }
                }
            }
        }

        private void PreShoot()
        {
            if (ammo > 0)
            {
                Shoot();
            }
            else
            {
                OnNoAmmo();
                ResetCoolDown();
            }
        }

        private bool TryRaycastBullet(Rectangle bulletRect, int i, float boundX)
        {
            bulletRect.X = i;
            foreach (Enemy item in Game1.EnemyContainer.Enemies)
            {
                if (bulletRect.Intersects(item.GetCollider()))
                {
                    item.Hit(Damage);
                    return true;
                }
            }
            return false;
        }

        public virtual void Shoot()
        {
            frame = 0;
            ammo--;
            ResetCoolDown();

            if (Game1.EnemyContainer.Count > 0)
            {
                Rectangle bulletRect = _bulletRectStart;
                if (player.Direction == 1)
                {
                    float rightSideBound = (Game1.GamePlayCamera.Position.X + Game.Window.ClientBounds.Width / 2);
                    for (int i = _bulletRectStart.X; i < rightSideBound; i++)
                        if (TryRaycastBullet(bulletRect, i, rightSideBound) == true)
                            return;
                }
                else
                {
                    float leftSideBound = (Game1.GamePlayCamera.Position.X + Game.Window.ClientBounds.Width / 2);
                    for (int i = _bulletRectStart.X; i > _bulletRectStart.X - leftSideBound; i--)
                        if (TryRaycastBullet(bulletRect, i, leftSideBound) == true)
                            return;
                }
            }
        }

        public void AddAmmo(int count)
        {
            ammo += count;
        }

        private void ResetCoolDown()
        {
            _shootingCoolDown = shootingCoolDown;
            ableToShoot = false;
        }
        public virtual void OnNoAmmo()
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (player.Direction == 1)
            {
                spriteBatch.Draw(Game1.Textures[weaponAssetName], player.Position + weaponOffsetRight, Game1.SpriteSheets[weaponSpriteSheetName][frame], Color.White, 0, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)player.Position.X + (int)shootPointOffsetRight.X + Game1.SpriteSheets[weaponSpriteSheetName][frame].Width / 2, (int)player.Position.Y + (int)shootPointOffsetRight.Y + Game1.SpriteSheets[weaponSpriteSheetName][frame].Height / 2 + 2, 5, 5);
                DrawShootingPoint(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(Game1.Textures[weaponAssetName], player.Position - weaponOffsetLeft, Game1.SpriteSheets[weaponSpriteSheetName][frame], Color.White, 0, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)player.Position.X - (int)shootPointOffsetLeft.X - Game1.SpriteSheets[weaponSpriteSheetName][frame].Width / 2, (int)player.Position.Y + (int)shootPointOffsetLeft.Y + Game1.SpriteSheets[weaponSpriteSheetName][frame].Height / 2 + 2, 5, 5);
                DrawShootingPoint(spriteBatch);
            }
        }

        private void DrawShootingPoint(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(Game1.Textures["1x1"], _bulletRectStart, Color.Red);
#endif
        }
    }
}

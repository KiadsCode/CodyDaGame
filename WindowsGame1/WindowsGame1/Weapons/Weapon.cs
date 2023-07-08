using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.Weapons
{
    public class Weapon
    {
        private int _frame = 0;
        private int _ammo = 0;
        private int _animationCoolDown = 2;
        private int _shootingCoolDown = 3;
        private int _resetShootingCoolDown = 3;
        private ushort _damage = 0;
        private bool _ableToShoot = false;
        private SpriteEffects _spriteEffects = SpriteEffects.None;
        private Rectangle _bulletRectStart = Rectangle.Empty;
        private MouseState _oldMouseState = Mouse.GetState();
        private Player _player = default(Player);
        private bool _automatical = false;

        protected string WeaponAssetName = string.Empty;
        protected string WeaponSpriteSheetName = string.Empty;
        protected WeaponOffset WeaponOffset = new WeaponOffset(Vector2.Zero);
        protected WeaponOffset ShootingPointOffset = new WeaponOffset(Vector2.Zero);

        internal Game Game = null;

        public bool Automatical
        {
            get
            {
                return _automatical;
            }
            protected set
            {
                _automatical = value;
            }
        }
        public ushort Damage
        {
            get
            {
                return _damage;
            }
            protected set
            {
                _damage = value;
            }
        }
        public int ShootingCoolDown
        {
            get
            {
                return _resetShootingCoolDown;
            }
            protected set
            {
                _resetShootingCoolDown = value;
            }
        }
        public int AmmoCount
        {
            get
            {
                return _ammo;
            }
            protected set
            {
                _ammo = value;
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
            _frame = Game1.SpriteSheets[WeaponSpriteSheetName].FramesCount - 1; 
        }

        public virtual void Update(Player player, GameTime gameTime)
        {
            // TODO: Add your update code here
            _player = player;
            MouseState mouseState = Mouse.GetState();
            if (_shootingCoolDown > 0)
                _shootingCoolDown--;
            TryShoot(mouseState);

            _animationCoolDown--;
            if (_animationCoolDown <= 0)
                if (_frame <= Game1.SpriteSheets[WeaponSpriteSheetName].FramesCount - 2)
                    _frame++;
            switch (player.Direction)
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

        protected void InitializeWeaponProperties(int ammoCount, ushort damage, int shootingCoolDown, bool automatical, string weaponAssetName, string weaponSpriteSheetName, WeaponOffset weaponOffset, WeaponOffset shootingPointOffset)
        {
            AmmoCount = ammoCount;
            Damage = damage;
            ShootingCoolDown = shootingCoolDown;
            Automatical = automatical;

            WeaponAssetName = weaponAssetName;
            WeaponSpriteSheetName = weaponSpriteSheetName;

            WeaponOffset = weaponOffset;
            ShootingPointOffset = shootingPointOffset;
        }

        private void TryShoot(MouseState mouseState)
        {
            _ableToShoot = _shootingCoolDown == 0;
            if (_ableToShoot)
            {
                bool shootRequired = false;
                shootRequired = mouseState.LeftButton == ButtonState.Pressed && _automatical == true
                    || mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released && _automatical == false;
                if (shootRequired)
                    PreShoot();
            }
        }

        private void PreShoot()
        {
            if (_ammo > 0)
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
            _frame = 0;
            _ammo--;
            ResetCoolDown();

            if (Game1.EnemyContainer.Count > 0)
            {
                Rectangle bulletRect = _bulletRectStart;
                if (_player.Direction == 1)
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
            _ammo += count;
        }

        private void ResetCoolDown()
        {
            _shootingCoolDown = _resetShootingCoolDown;
            _ableToShoot = false;
        }
        public virtual void OnNoAmmo()
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_player.Direction == 1)
            {
                spriteBatch.Draw(Game1.Textures[WeaponAssetName], _player.Position + WeaponOffset.Right, Game1.SpriteSheets[WeaponSpriteSheetName][_frame], Color.White, 0, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)_player.Position.X + (int)ShootingPointOffset.Right.X + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2, (int)_player.Position.Y + (int)ShootingPointOffset.Right.Y + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Height / 2 + 2, 5, 5);
                DrawShootingPoint(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(Game1.Textures[WeaponAssetName], _player.Position - WeaponOffset.Left, Game1.SpriteSheets[WeaponSpriteSheetName][_frame], Color.White, 0, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)_player.Position.X - (int)ShootingPointOffset.Left.X - Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2, (int)_player.Position.Y + (int)ShootingPointOffset.Left.Y + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Height / 2 + 2, 5, 5);
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

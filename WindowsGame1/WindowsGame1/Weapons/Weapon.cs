using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Engine.Collision;
using WindowsGame1.Engine.Map;

namespace WindowsGame1.Weapons
{
    public class Weapon
    {
        private int _frame = 0;
        private int _ammo = 0;
        private int _shootingCoolDown = 3;
        private int _resetShootingCoolDown = 3;
        private ushort _damage = 0;
        private bool _ableToShoot = false;
        private SpriteEffects _spriteEffects = SpriteEffects.None;
        private Rectangle _bulletRectStart = Rectangle.Empty;
        private MouseState _oldMouseState = Mouse.GetState();
        private Player _player = default(Player);
        private bool _automatical = false;
        private GamePadState _gamePadState = GamePad.GetState(PlayerIndex.One);
        private GamePadState _oldGamePadState = GamePad.GetState(PlayerIndex.One);
        private float _rotation = 0.0f;

        protected string WeaponAssetName = string.Empty;
        protected string WeaponSpriteSheetName = string.Empty;
        protected WeaponOffset WeaponOffset = new WeaponOffset(Vector2.Zero);
        protected WeaponOffset ShootingPointOffset = new WeaponOffset(Vector2.Zero);

        internal Game Game = null;

        public float Rotation
        {
            get { return _rotation; }
            set { if (value != null) _rotation = value; }
        }

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
            Game = game;
            Initialize();
        }

        public virtual void Initialize()
        {
            ResetAnimation();
        }

        public void ResetAnimation()
        {
            _frame = Game1.SpriteSheets[WeaponSpriteSheetName].FramesCount - 1;
            _rotation = 0.0f;
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        public virtual void Update(Player player, GameTime gameTime)
        {
            _player = player;
            MouseState mouseState = Mouse.GetState();
            _gamePadState = GamePad.GetState(PlayerIndex.One);
            if (_shootingCoolDown > 0)
                _shootingCoolDown--;
            TryShoot(mouseState);

            if (_frame <= Game1.SpriteSheets[WeaponSpriteSheetName].FramesCount - 2)
                _frame++;
            else
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);

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
            _oldGamePadState = _gamePadState;
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
                if (_gamePadState.IsConnected == false)
                    shootRequired = mouseState.LeftButton == ButtonState.Pressed && _automatical == true
                        || mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released && _automatical == false;
                else

                    shootRequired = _gamePadState.Triggers.Right != 0 && _automatical == true
                        || _gamePadState.Triggers.Right != 0 && _oldGamePadState.Triggers.Right != 1.0f && _automatical == false;

                if (shootRequired)
                    PreShoot();

                if (mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released && _ammo <= 0)
                    OnNoAmmo();
            }
        }

        private void PreShoot()
        {
            if (_ammo > 0)
                Shoot();
            else
                ResetCoolDown();
        }

        // TODO: Оптимизировать код стрельбы
        private bool TryRaycastBullet(Rectangle bulletRect, int i, float boundX)
        {
            bulletRect.X = i;

            List<IHitable> availableTargets = new List<IHitable>();

            foreach (Enemy item in Game1.MapComponent.Enemies)
                if (item.GetCollider().Intersects(Game1.GamePlayCamera.GetRectangle(Game)))
                    availableTargets.Add(item);

            foreach (Block item in Game1.MapComponent.Blocks)
            {
                if (item.IsHitableBlock())
                {
                    if (item.GetCollider().Intersects(Game1.GamePlayCamera.GetRectangle(Game)))
                        availableTargets.Add(item);
                }
            }

            foreach (IHitable item in availableTargets)
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
#if !DEBUG
            _ammo--;
#endif
            GamePad.SetVibration(PlayerIndex.One, 0.3f, 0.3f);
            ResetCoolDown();

            if (Game1.MapComponent.EnemiesCount > 0)
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
                int x = (int)_player.Position.X + (int)ShootingPointOffset.Right.X + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2;
                spriteBatch.Draw(Game1.Textures[WeaponAssetName], _player.Position + WeaponOffset.Right, Game1.SpriteSheets[WeaponSpriteSheetName][_frame], Color.White, -_rotation, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)_player.Position.X, (int)_player.Position.Y + (int)ShootingPointOffset.Right.Y + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Height / 2 + 2, 5, 5);
                DrawShootingPoint(spriteBatch);
            }
            else
            {
                int x = (int)_player.Position.X - (int)ShootingPointOffset.Left.X - Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2;
                spriteBatch.Draw(Game1.Textures[WeaponAssetName], _player.Position - WeaponOffset.Left, Game1.SpriteSheets[WeaponSpriteSheetName][_frame], Color.White, _rotation, Vector2.Zero, 1, _spriteEffects, 0);
                _bulletRectStart = new Rectangle((int)_player.Position.X, (int)_player.Position.Y + (int)ShootingPointOffset.Left.Y + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Height / 2 + 2, 5, 5);
                DrawShootingPoint(spriteBatch);
            }
        }

        private void DrawShootingPoint(SpriteBatch spriteBatch)
        {
#if DEBUG
            Rectangle rectRight = new Rectangle(
                    _bulletRectStart.X + (int)ShootingPointOffset.Right.X + Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2 + 2,
                    _bulletRectStart.Y,
                    _bulletRectStart.Width,
                    _bulletRectStart.Height);
            Rectangle rectLeft = new Rectangle(
                    _bulletRectStart.X - (int)ShootingPointOffset.Left.X - Game1.SpriteSheets[WeaponSpriteSheetName][_frame].Width / 2 + 2,
                    _bulletRectStart.Y,
                    _bulletRectStart.Width,
                    _bulletRectStart.Height);
            if (_player.Direction == 1)
                spriteBatch.Draw(Game1.Textures["1x1"],
                    rectRight,
                    Color.Red);
            else
                spriteBatch.Draw(Game1.Textures["1x1"],
                    rectLeft,
                    Color.Red);
#endif
        }
    }
}

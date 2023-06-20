using Microsoft.Xna.Framework;

namespace WindowsGame1.Weapons
{
    public class Magnum : Weapon
    {
        public Magnum(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            weaponAssetName = "pistolShot";
            weaponSpriteSheetName = "pistolShot";
            ammo = 15;
            automatical = false;
            shootingCoolDown = 20;
            weaponOffsetRight = new Vector2(20, -35);
            shootPointOffsetRight = new Vector2(16, -35);
            weaponOffsetLeft = new Vector2(130, 35);
            shootPointOffsetLeft = new Vector2(20, -35);
            damage = 20;
            base.Initialize();
        }

        public override void Shoot()
        {
            Game1.SoundEffects["magnum"].Play();
            base.Shoot();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["magnumNoAmmo"].Play();
            base.OnNoAmmo();
        }
    }
}

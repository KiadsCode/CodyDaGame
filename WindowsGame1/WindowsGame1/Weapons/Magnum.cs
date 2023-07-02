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
            AmmoCount = 15;
            Damage = 20;
            ShootingCoolDown = 20;
            Automatical = false;

            WeaponAssetName = "pistolShot";
            WeaponSpriteSheetName = "pistolShot";

            WeaponOffset=new Weapons.WeaponOffset(new Vector2(130, 35), new Vector2(20, -35));
            ShootingPointOffset = new Weapons.WeaponOffset(new Vector2(20, -35), new Vector2(16, -35));
            base.Initialize();
        }

        public override void Shoot()
        {
            Game1.SoundEffects["magnum"].Play();
            base.Shoot();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["weaponNoAmmo"].Play();
            base.OnNoAmmo();
        }
    }
}

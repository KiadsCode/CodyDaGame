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
            InitializeWeaponProperties(
                15,
                20,
                20,
                false,
                "pistolShot",
                "pistolShot",
                new WeaponOffset(new Vector2(130, 35), new Vector2(20, -35)),
                new WeaponOffset(new Vector2(20, -35), new Vector2(16, -35))
                );
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

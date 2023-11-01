using Microsoft.Xna.Framework;

namespace WindowsGame1.Weapons
{
    public class Smg : Weapon
    {
        public Smg(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            InitializeWeaponProperties(
                75,
                4,
                3,
                true,
                "smgShot",
                "smgShot",
                new WeaponOffset(new Vector2(145, 36), new Vector2(20, -36)),
                new WeaponOffset(new Vector2(24, -40), new Vector2(16, -40))
                );
            base.Initialize();
        }

        public override void Shoot()
        {
            Game1.SoundEffects["smg"].Play();
            base.Shoot();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["weaponNoAmmo"].Play();
            base.OnNoAmmo();
        }
    }
}

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
                102,
                3,
                3,
                true,
                "smgShot",
                "smgShot",
                new WeaponOffset(new Vector2(130, 30), new Vector2(20, -36)),
                new WeaponOffset(new Vector2(24, -35), new Vector2(16, -35))
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

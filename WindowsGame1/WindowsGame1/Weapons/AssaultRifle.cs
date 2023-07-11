using Microsoft.Xna.Framework;

namespace WindowsGame1.Weapons
{
    public class AssaultRifle : Weapon
    {
        public AssaultRifle(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            InitializeWeaponProperties(
                60,
                10,
                5,
                true,
                "arShot",
                "arShot",
                new WeaponOffset(new Vector2(157, 32), new Vector2(-16, -32)),
                new WeaponOffset(new Vector2(6, -30), new Vector2(3, -30))
                );

            base.Initialize();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["weaponNoAmmo"].Play();
            base.OnNoAmmo();
        }

        public override void Shoot()
        {
            Game1.SoundEffects["arShotC"].Play();
            base.Shoot();
        }
    }
}

using Microsoft.Xna.Framework;

namespace WindowsGame1.Weapons
{
    public class MagnumSilencer : Weapon
    {
        public MagnumSilencer(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            InitializeWeaponProperties(
                24,
                13,
                10,
                false,
                "pistolSilencerShot",
                "pistolSilencerShot",
                new WeaponOffset(new Vector2(130, 35), new Vector2(20, -35)),
                new WeaponOffset(new Vector2(55, -34), new Vector2(41, -34))
                );
            base.Initialize();
        }

        public override void Shoot()
        {
            Game1.SoundEffects["magnumSilencer"].Play();
            base.Shoot();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["weaponNoAmmo"].Play();
            base.OnNoAmmo();
        }
    }
}

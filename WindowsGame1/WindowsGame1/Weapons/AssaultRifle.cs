using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame1.Weapons
{
    public class AssaultRifle : Weapon
    {
        private Random _random = new Random();

        public AssaultRifle(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            weaponAssetName = "arShot";
            weaponSpriteSheetName = "arShot";
            ammo = 60;
            shootingCoolDown = 5;
            automatical = true;
            damage = 5;
            weaponOffsetRight = new Vector2(-16, -32);
            shootPointOffsetRight = new Vector2(3, -30);
            weaponOffsetLeft = new Vector2(157, 32);
            shootPointOffsetLeft = new Vector2(6, -30);
            base.Initialize();
        }

        public override void OnNoAmmo()
        {
            Game1.SoundEffects["arNoAmmo"].Play();
            base.OnNoAmmo();
        }

        public override void Shoot()
        {
            SoundEffect[] sfxs = { Game1.SoundEffects["arShotB"], Game1.SoundEffects["arShotA"] };
            int sfxIndex = _random.Next(0, sfxs.Length);
            sfxs[sfxIndex].Play();
            base.Shoot();
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Enemy
    {
        private int _health = 100;
        private short _direction = 1;
        private bool _alive = true;
        private Vector2 _position = Vector2.Zero;
        private SpriteEffects _spriteEffects = SpriteEffects.None;
        private Random _random = new Random();

        public short Direction
        {
            get
            {
                return _direction;
            }
        }
        public bool Alive
        {
            get
            {
                return _alive;
            }
        }
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public Enemy(Vector2 position)
        {
            _position = position;
        }

        public Rectangle GetCollider()
        {
            return new Rectangle((int)_position.X - Game1.Textures["enemy"].Width / 2, (int)_position.Y - Game1.Textures["enemy"].Height / 2, Game1.Textures["enemy"].Width, Game1.Textures["enemy"].Height);
        }

        public void Hit(int damage)
        {
            _health -= damage;
            _alive = _health > 0;
            if (_alive == false)
            {
                SoundEffect[] soundeffects = { Game1.SoundEffects["killsoundA"], Game1.SoundEffects["killsoundB"] };
                int randomSfx = _random.Next(0, soundeffects.Length);
                soundeffects[randomSfx].Play();
            }
        }

        public void Update(GameTime gameTime)
        {
            bool playerAfterEnemy = Game1.Player.Position.X > _position.X;
            _direction = Convert.ToInt16(!playerAfterEnemy);
            _spriteEffects = (SpriteEffects)_direction;
        }

        public void Draw(SpriteBatch sp)
        {
            if (_alive)
                sp.Draw(Game1.Textures["enemy"], _position, null, Color.White, 0, new Vector2(Game1.Textures["enemy"].Width / 2, Game1.Textures["enemy"].Height / 2), 1, _spriteEffects, 0);
        }

    }
}

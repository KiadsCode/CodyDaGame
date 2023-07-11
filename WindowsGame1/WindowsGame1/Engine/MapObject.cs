using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine
{
    public class Block : IHitable
    {
        private Vector2 _position = Vector2.Zero;
        private int _type = 0;
        private int _health = 60;
        private bool _alive = true;

        public bool Alive
        {
            get
            {
                return _alive;
            }
        }

        public Block(int type, Vector2 position)
        {
            _type = type;
            _position = position;
        }

        public void Hit(int damage)
        {
            if (_type != 0)
            {
                _health -= damage;
                System.Console.WriteLine(_health);
                _alive = _health > 0;
                if (_alive == false)
                    Game1.SoundEffects["explode"].Play();
            }
        }

        public Rectangle GetCollider()
        {
            Texture2D[] textures = { Game1.Textures["solidBlock"], Game1.Textures["weakBlock"] };
            int textureWidth = textures[_type].Width;
            int textureHeight = textures[_type].Height;
            return new Rectangle((int)_position.X - textureWidth / 2, (int)_position.Y - textureHeight / 2, textureWidth, textureHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D[] textures = { Game1.Textures["solidBlock"], Game1.Textures["weakBlock"] };
            spriteBatch.Draw(textures[_type], _position, null, Color.White, 0, new Vector2(textures[_type].Width / 2, textures[_type].Height / 2), 1.0f, SpriteEffects.None, 0);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using WindowsGame1.Engine;
using WindowsGame1.Engine.Collision;

namespace WindowsGame1
{
    public class Enemy : IHitable
    {
        public const int MovementSpeed = 2;

        private int _health = 100;
        private short _direction = 1;
        private bool _alive = true;
        private bool _playerContacted = false;

        private Player Player
        {
            get
            {
                return Game1.Player;
            }
        }

        private Vector2 _position = Vector2.Zero;
        private Circle _circle = new Circle(Vector2.Zero, 160);
        private SpriteEffects _spriteEffects = SpriteEffects.None;

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
            return new Rectangle(
                (int)_position.X - Game1.Textures["enemy"].Width / 2,
                (int)_position.Y - Game1.Textures["enemy"].Height / 2,
                Game1.Textures["enemy"].Width,
                Game1.Textures["enemy"].Height);
        }

        public void Hit(int damage)
        {
            _health -= damage;
            _alive = _health > 0;
            if (_alive == false)
                Game1.SoundEffects["killsoundA"].Play();
        }

        public void Update()
        {
            OnContactEvent();

            if (_circle.Intersects(Game1.Player.GetCollider()))
                _playerContacted = true;
        }

        private void OnContactEvent()
        {
            if (_playerContacted)
            {
                bool enemyAfterPlayer = _position.X > Player.Position.X;
                _circle.Center = _position;
                _direction = Convert.ToInt16(enemyAfterPlayer);
                _spriteEffects = (SpriteEffects)_direction;

                MoveTowardsPlayer();
            }
        }

        private void MoveTowardsPlayer()
        {
            float enemyToPlayerDistance = Player.Position.X - _position.X;
            if (enemyToPlayerDistance > 0)
            {
                if (enemyToPlayerDistance > 300)
                    if (HasPotentialCollision(new Vector2(MovementSpeed, 0)) == false)
                        _position.X += MovementSpeed;

                if (enemyToPlayerDistance < 200)
                    if (HasPotentialCollision(new Vector2(-MovementSpeed, 0)) == false)
                        _position.X += -MovementSpeed;
            }
            if (enemyToPlayerDistance < 0)
            {
                if (enemyToPlayerDistance > -200)
                    if (HasPotentialCollision(new Vector2(MovementSpeed, 0)) == false)
                        _position.X += MovementSpeed;

                if (enemyToPlayerDistance < -300)
                    if (HasPotentialCollision(new Vector2(-MovementSpeed, 0)) == false)
                        _position.X -= MovementSpeed;
            }
        }

        private bool HasPotentialCollision(Vector2 direction)
        {
            Rectangle potentialCollider = GetCollider();
            potentialCollider.X += (int)direction.X;
            potentialCollider.Y += (int)direction.Y;

            foreach (IHitable item in Game1.MapComponent.Blocks)
                if (potentialCollider.Intersects(item.GetCollider()))
                    return true;

            return false;
        }

        public void Draw(SpriteBatch sp)
        {
            if (_alive)
                sp.Draw(Game1.Textures["enemy"],
                    _position,
                    null,
                    Color.White,
                    0,
                    new Vector2(Game1.Textures["enemy"].Width / 2,
                    Game1.Textures["enemy"].Height / 2),
                    1,
                    _spriteEffects,
                    0);
        }

    }
}

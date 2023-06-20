using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class EnemyContainer : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private List<Enemy> _enemies;

        public IEnumerable<Enemy> Enemies
        {
            get
            {
                return _enemies;
            }
        }

        public int Count
        {
            get
            {
                return _enemies.Count;
            }
        }

        public EnemyContainer(Game game)
            : base(game)
        {
            DrawOrder = 0;
            _enemies = new List<Enemy>();
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void Remove(int index)
        {
            if (index < _enemies.Count)
                _enemies.RemoveAt(index);
        }

        public void Add(Vector2 position)
        {
            Enemy enemy = new Enemy(position);
            _enemies.Add(enemy);
        }

        public override void Update(GameTime gameTime)
        {
            if (_enemies.Count > 0)
            {
                foreach (Enemy item in _enemies)
                    item.Update(gameTime);
                for (int i = 0; i < _enemies.Count; i++)
                    if (_enemies[i].Alive == false)
                        Remove(i);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Game1.GamePlayCamera.GetMatrix(Game.GraphicsDevice));
            if (_enemies.Count > 0)
            {
                int fov = 95;
                foreach (Enemy item in _enemies)
                {
                    float enemyToCameraDistance = Vector2.Distance(item.Position, Game1.GamePlayCamera.Position);
                    if (enemyToCameraDistance < (Game.Window.ClientBounds.Width / 2) + fov || enemyToCameraDistance < (Game.Window.ClientBounds.Height / 2) + fov)
                        item.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

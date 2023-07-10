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
        }

        public void SetData(Enemy[] vs)
        {
            _enemies = new List<Enemy>(vs);
        }

        public void SetData(List<Enemy> vs)
        {
            _enemies = new List<Enemy>(vs);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public void Remove(int index)
        {
            if (index < _enemies.Count)
                _enemies.RemoveAt(index);
        }

        public void KillNiggers()
        {
            _enemies.Clear();
        }

        public void Add(Vector2 position)
        {
            Enemy enemy = new Enemy(position);
            _enemies.Add(enemy);
        }

        public void Add()
        {
            Enemy enemy = new Enemy(Vector2.Zero);
            _enemies.Add(enemy);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Enemy item in _enemies)
                item.Update();
            for (int i = 0; i < _enemies.Count; i++)
                if (_enemies[i].Alive == false)
                    Remove(i);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Game1.GamePlayCamera.GetMatrix(Game.GraphicsDevice));
            DrawEnemies();
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawEnemies()
        {
            foreach (Enemy item in _enemies)
            {
                Rectangle cameraRectangle = Game1.GamePlayCamera.GetRectangle(Game);
                Rectangle enemyRectangle = item.GetCollider();
                if (enemyRectangle.Intersects(cameraRectangle))
                    item.Draw(_spriteBatch);
            }
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine.Map
{
    public class Map : DrawableGameComponent
    {
        private List<Enemy> _enemies;
        private List<Block> _blocks;
        private SpriteBatch _spriteBatch;

        public IEnumerable<Enemy> Enemies
        {
            get
            {
                return _enemies;
            }
        }

        public IEnumerable<Block> Blocks
        {
            get
            {
                return _blocks;
            }
        }

        public int EnemiesCount
        {
            get
            {
                return _enemies.Count;
            }
        }

        public int BlocksCount
        {
            get
            {
                return _blocks.Count;
            }
        }

        public Map(Game game)
            : base(game)
        {
            _blocks = new List<Block>();
            _enemies = new List<Enemy>();
        }

        public void UpdateData(MapData data)
        {
            _blocks = data.Blocks;
            _enemies = data.Enemies;
        }

        public void Load(string map)
        {
            _enemies.Clear();
            _blocks.Clear();
            MapData extractedDataMap = MapProcessor.Process(Game, map);
            UpdateData(extractedDataMap);
            foreach (Block item in _blocks)
            {
                if (item.Type == 3)
                {
                    Game1.Player.SetPosition(item.Position);
                    break;
                }
            }
        }

        public void LoadFromPath(string map)
        {
            _enemies.Clear();
            _blocks.Clear();
            MapData extractedDataMap = MapProcessor.Process(map);
            UpdateData(extractedDataMap);
            foreach (Block item in _blocks)
            {
                if (item.Type == 3)
                {
                    Game1.Player.SetPosition(item.Position);
                    break;
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Enemy item in _enemies)
                item.Update();
            for (int i = 0; i < _enemies.Count; i++)
                if (_enemies[i].Alive == false)
                    _enemies.RemoveAt(i);
            for (int i = 0; i < _blocks.Count; i++)
                if (_blocks[i].Alive == false)
                    _blocks.RemoveAt(i);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Block[] availableObjects = GetAvailableObjects();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Game1.GamePlayCamera.GetMatrix(Game.GraphicsDevice));

            foreach (Block item in availableObjects)
                item.Draw(_spriteBatch);

            foreach (Enemy item in _enemies)
            {
                Rectangle cameraRectangle = Game1.GamePlayCamera.GetRectangle(Game);
                Rectangle enemyRectangle = item.GetCollider();
                if (enemyRectangle.Intersects(cameraRectangle))
                    item.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public Block[] GetAvailableObjects()
        {
            Rectangle cameraViewport = Game1.GamePlayCamera.GetRectangle(Game);
            List<Block> objects = new List<Block>();
            foreach (Block item in _blocks)
                if (item.GetCollider().Intersects(cameraViewport))
                    objects.Add(item);
            return objects.ToArray();
        }
    }
}

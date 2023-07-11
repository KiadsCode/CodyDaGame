using System.Collections.Generic;

namespace WindowsGame1.Engine
{
    public class MapData
    {
        public List<Block> Blocks;
        public List<Enemy> Enemies;

        public MapData(List<Block> blocks, List<Enemy> enemies)
        {
            Enemies = enemies;
            Blocks = blocks;
        }

        public MapData()
        {
            Enemies = new List<Enemy>();
            Blocks = new List<Block>();
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SparrowV2
{
    public struct SpriteSheet
    {
        private Dictionary<int, Rectangle> _animation;
        private string _name;

        public int FramesCount
        {
            get
            {
                return _animation.Count;
            }
        }

        public bool ContainsFrame(int frameID)
        {
            return _animation.ContainsKey(frameID);
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public SpriteSheet(Dictionary<int, Rectangle> animation, string name)
        {
            _animation = animation;
            _name = name;
        }

        public Rectangle this[int frameID]
        {
            get
            {
                if (_animation.ContainsKey(frameID))
                    return _animation[frameID];
                return new Rectangle(0, 0, 0, 0);
            }
        }
    }
}

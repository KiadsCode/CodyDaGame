using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SparrowV2
{
    public struct SpriteSheet
    {
        private Dictionary<string, Rectangle> _animation;
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
            Rectangle[] animationRectangles = new Rectangle[_animation.Values.Count];
            _animation.Values.CopyTo(animationRectangles, 0);
            return animationRectangles[frameID] != null;
        }
        public bool ContainsFrame(string frameName)
        {
            return _animation.ContainsKey(frameName);
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public SpriteSheet(Dictionary<string, Rectangle> animation, string name)
        {
            _animation = animation;
            _name = name;
        }

        public Rectangle this[int frameID]
        {
            get
            {
                Rectangle[] animationRectangles = new Rectangle[_animation.Values.Count];
                _animation.Values.CopyTo(animationRectangles, 0);

                if (ContainsFrame(frameID))
                    return animationRectangles[frameID];
                return Rectangle.Empty;
            }
        }

        public Rectangle this[string frameName]
        {
            get
            {
                if (ContainsFrame(frameName))
                    return _animation[frameName];
                return Rectangle.Empty;
            }
        }
    }
}

using Microsoft.Xna.Framework;

namespace WindowsGame1.Weapons
{
    public class WeaponOffset
    {
        private Vector2[] _sides = { Vector2.Zero, Vector2.Zero };

        public Vector2 Left
        {
            get
            {
                return _sides[0];
            }
            set
            {
                _sides[0] = value;
            }
        }
        public Vector2 Right
        {
            get
            {
                return _sides[1];
            }
            set
            {
                _sides[1] = value;
            }
        }

        public WeaponOffset(Vector2 left, Vector2 right)
        {
            Left = left;
            Right = right;
        }

        public WeaponOffset(Vector2 position)
        {
            Left = position;
            Right = position;
        }
    }
}

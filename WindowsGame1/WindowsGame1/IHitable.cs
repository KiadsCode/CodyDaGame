using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    interface IHitable
    {
        void Hit(int damage);
        Rectangle GetCollider();
    }
}

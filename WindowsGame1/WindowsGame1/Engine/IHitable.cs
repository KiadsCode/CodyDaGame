using Microsoft.Xna.Framework;

namespace WindowsGame1.Engine
{
    public interface IHitable
    {
        void Hit(int damage);
        Rectangle GetCollider();
    }
}

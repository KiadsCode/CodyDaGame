using Microsoft.Xna.Framework;

namespace WindowsGame1.Engine.Collision
{
    public interface IHitable
    {
        void Hit(int damage);
        Rectangle GetCollider();
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Engine.Collision
{
    public class Polygon2D
    {
        private List<Vector2> _points = new List<Vector2>();

        public Vector2[] Points
        {
            get
            {
                return _points.ToArray();
            }
        }

        public Polygon2D()
        {
            _points = new List<Vector2>();
        }

        public Polygon2D(Vector2[] points)
        {
            _points = new List<Vector2>(points);
        }

        public void Add(Vector2 point)
        {
            _points.Add(point);
        }

        public void Remove(int index)
        {
            _points.RemoveAt(index);
        }

        public bool Intersects(Polygon2D poly)
        {
            Gjk gjk = new Gjk(this, poly);
            return gjk.CheckCollision();
        }
    }
}

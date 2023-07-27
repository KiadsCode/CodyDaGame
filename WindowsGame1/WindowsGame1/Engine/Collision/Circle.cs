using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine
{
    public struct Circle
    {
        public Vector2 Center;
        public int Radius;

        public Circle(Vector2 position, int radius)
        {
            Center = position;
            Radius = radius;
        }

        public bool Intersects(Rectangle rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = Center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = GetVisualization(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(texture, Center, null, Color.Black, 0, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0);
        }

        private Texture2D GetVisualization(GraphicsDevice graphicsDevice)
        {
            int radius = Radius * 2;
            Texture2D texture = new Texture2D(graphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}

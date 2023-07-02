using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine
{
    public class Camera2D
    {
        private Matrix _transform = Matrix.Identity;

        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Vector2 Position { get; set; }

        public Matrix GetMatrix(GraphicsDevice graphicsDevice)
        {
            _transform =
              Matrix.CreateTranslation(
              new Vector3(-Position.X, -Position.Y, 0)) *
              Matrix.CreateRotationZ(Rotation) *
              Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
              Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }

        public void Lerp(Vector2 position, float speed)
        {
            Position = Vector2.Lerp(Position, position, speed);
        }

        public Camera2D()
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
        }
    }
}
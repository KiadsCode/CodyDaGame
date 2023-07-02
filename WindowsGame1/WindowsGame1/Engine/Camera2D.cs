using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine
{
    public class Camera
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

        public Rectangle GetRectangle(Game game)
        {
            Rectangle rectangle = Rectangle.Empty;
            rectangle = new Rectangle(
                Convert.ToInt32(Position.X) - game.Window.ClientBounds.Width / 2,
                Convert.ToInt32(Position.Y) - game.Window.ClientBounds.Height / 2,
                game.Window.ClientBounds.Width,
                game.Window.ClientBounds.Height);
            return rectangle;
        }

        public void Lerp(Vector2 position, float speed)
        {
            Position = Vector2.Lerp(Position, position, speed);
        }

        public Camera()
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
        }
    }
}
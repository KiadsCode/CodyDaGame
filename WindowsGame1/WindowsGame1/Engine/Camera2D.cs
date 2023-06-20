using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine
{
    public class Camera2D
    {
        private float _zoom = 1.0f;
        private float _rotation = 0.0f;
        private Matrix _transform = Matrix.Identity;
        private Vector2 _position = Vector2.Zero;

        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                    _zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Matrix GetMatrix(GraphicsDevice graphicsDevice)
        {
            _transform =
              Matrix.CreateTranslation(
              new Vector3(-_position.X, -_position.Y, 0)) *
              Matrix.CreateRotationZ(Rotation) *
              Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
              Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }

        public void Lerp(Vector2 positionB, float speed)
        {
            _position = Vector2.Lerp(_position, positionB, speed);
        }

        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }
    }
}
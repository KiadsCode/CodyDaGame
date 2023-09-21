using System;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine.Discord
{
    public partial struct ImageHandle
    {
        static public ImageHandle User(Int64 id)
        {
            ImageHandle ih = User(id, 128);
            return ih;
        }

        static public ImageHandle User(Int64 id, UInt32 size)
        {
            return new ImageHandle
            {
                Type = ImageType.User,
                Id = id,
                Size = size,
            };
        }
    }

    public partial class ImageManager
    {
        public void Fetch(ImageHandle handle, FetchHandler callback)
        {
            Fetch(handle, false, callback);
        }

        public byte[] GetData(ImageHandle handle)
        {
            var dimensions = GetDimensions(handle);
            var data = new byte[dimensions.Width * dimensions.Height * 4];
            GetData(handle, data);
            return data;
        }

        public Texture2D GetTexture(ImageHandle handle, GraphicsDevice device)
        {
            ImageDimensions dimensions = GetDimensions(handle);
            Texture2D texture = new Texture2D(device, (int)dimensions.Width, (int)dimensions.Height);
            texture.SetData(GetData(handle));
            return texture;
        }
    }
}

using System;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SparrowV2
{
    public class SpriteSheetImporter
    {
        public static SpriteSheet Import(ContentManager content, string filename)
        {
            // TODO: read the specified file into an instance of the imported type.
            string path = string.Format("{0}\\{1}", content.RootDirectory, filename);
            if (File.Exists(string.Format("{0}.xml", path)) == false)
                throw new Exception(string.Format("File \"{0}\" does not exist", filename));
            SpriteSheetProcessor processor = new SpriteSheetProcessor();
            SpriteSheet timp = processor.Process(path);
            return timp;
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace conkoy
{
    public class CConKoy
    {
        private static List<string[]> GetArray()
        {
            string[] directoriesToParse = { "images", "images\\xml", "sounds", "maps", "fonts", "shaders" };
            List<string[]> assets = new List<string[]>(directoriesToParse.Length);
            for (int i = 0; i < directoriesToParse.Length; i++)
                assets.Add(Directory.GetFiles(string.Format("Content\\{0}", directoriesToParse[i])));
            foreach (string[] item in assets)
                for (int i = 0; i < item.Length; i++)
                    item[i] = item[i].Replace('\\', '/');

            return assets;
        }

        public static void Write()
        {
            List<string[]> assets = GetArray();

            using (FileStream fs = File.Create("LoadingConfig.conf"))
            using (StreamWriter sw = new StreamWriter(fs))
                foreach (string[] array in assets)
                    for (int i = 0; i < array.Length; i++)
                        sw.WriteLine(array[i]);
        }
    }
}

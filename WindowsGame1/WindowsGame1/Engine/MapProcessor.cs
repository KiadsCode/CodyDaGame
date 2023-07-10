using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Engine
{
    public class MapProcessor
    {
        public const string MapExtension = "cdm";

        public static List<Enemy> Process(Game game, string mapPath)
        {
            List<Enemy> map = new List<Enemy>();
            string[] fileText = GetFileData(game, mapPath);

            foreach (string mapSegment in fileText)
            {
                if (HasSomething(mapSegment))
                {
                    Vector2 position = Vector2.Zero;
                    int type = 0;
                    string preParsedType = string.Empty;
                    string[] preParsedPosition = new string[2];
                    for (int i = 0; mapSegment[i] != ','; i++)
                        preParsedType += mapSegment[i];
                    preParsedType = preParsedType.Trim();
                    for (int i = preParsedType.Length + 1; mapSegment[i] != ','; i++)
                        preParsedPosition[0] += mapSegment[i];
                    for (int i = (preParsedType.Length + preParsedPosition[0].Length) + 2; mapSegment[i] != ';'; i++)
                        preParsedPosition[1] += mapSegment[i];
                    for (int i = 0; i < preParsedPosition.Length; i++)
                        preParsedPosition[i] = preParsedPosition[i].Trim();
                    type = Convert.ToInt32(preParsedType);
                    position = new Vector2(
                        Convert.ToInt32(preParsedPosition[0]),
                        Convert.ToInt32(preParsedPosition[1]));
                    Enemy enemy = new Enemy(position);
                    map.Add(enemy);
                }
            }
            return map;
        }

        private static string[] GetFileData(Game game, string mapPath)
        {
            string[] data = new string[1];
            string rawData = string.Empty;
            using (StreamReader stream = new StreamReader(TitleContainer.OpenStream(string.Format("{0}\\maps\\{1}.{2}", game.Content.RootDirectory, mapPath, MapExtension))))
                rawData = stream.ReadToEnd();
            data = rawData.Split('\n');
            for (int i = 0; i < data.Length; i++)
                data[i] = data[i].Trim();

            return data;
        }

        private static bool HasSomething(string data)
        {
            foreach (char item in data)
            {
                bool result = item != ' ';
                if (result)
                    return true;
            }
            return false;
        }
    }
}

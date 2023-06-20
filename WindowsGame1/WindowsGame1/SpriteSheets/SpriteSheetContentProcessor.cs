using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace SparrowV2
{
    public class SpriteSheetProcessor
    {
        public SpriteSheet Process(string input)
        {
            SpriteSheet sparrowAtlas = LoadSparrow(input);
            return sparrowAtlas;
        }

        private SpriteSheet LoadSparrow(string name)
        {
            string[] fileNameSplited = name.Split('\\', '.');
            string[] sparrowArray = SparrowToNative(name);
            Dictionary<int, Rectangle> outputDictionary = new Dictionary<int, Rectangle>();

            for (int jjasd = 0; jjasd < sparrowArray.Length - 1; jjasd++)
            {
                string rawDataShit = sparrowArray[jjasd];
                Rectangle parsedRectangle;
                int spacebarMoment = 0;
                int[] parsedValues = new int[4];
                string parsedName = string.Empty;
                string pstringInteger = string.Empty;

                for (int i = 0; i < rawDataShit.Length; i++)
                {
                    if (rawDataShit[i] != ':')
                        parsedName += rawDataShit[i];
                    else
                        break;
                }

                for (int i = rawDataShit.Length - 1; i >= parsedName.Length; i--)
                {
                    pstringInteger += rawDataShit[i];
                    if (rawDataShit[i] == ' ')
                    {
                        spacebarMoment = i;
                        break;
                    }
                }
                parsedValues[3] = System.Convert.ToInt32(ReverseString(pstringInteger));
                pstringInteger = string.Empty;

                for (int i = 2; i >= 0; i--)
                {
                    for (int j = spacebarMoment - 1; j >= parsedName.Length; j--)
                    {
                        pstringInteger += rawDataShit[j];
                        if (rawDataShit[j] == ' ')
                        {
                            spacebarMoment = j;
                            break;
                        }
                    }
                    parsedValues[i] = System.Convert.ToInt32(ReverseString(pstringInteger));
                    pstringInteger = string.Empty;
                }

                parsedRectangle = new Rectangle(parsedValues[0], parsedValues[1], parsedValues[2], parsedValues[3]);
                outputDictionary.Add(outputDictionary.Count, parsedRectangle);
            }
            string animationName = fileNameSplited[fileNameSplited.Length - 2];
            SpriteSheet ssfSpriteFile = new SpriteSheet(outputDictionary, animationName);
            return ssfSpriteFile;
        }
        private string ReverseString(string str)
        {
            string output = string.Empty;
            for (int i = str.Length - 1; i >= 0; i--)
                output += str[i];
            return output;
        }
        private string[] SparrowToNative(string fileToCompile)
        {
            string[] fileText = GetFileText(string.Format("{0}.xml", fileToCompile));
            string[] xmlArguments =
            {
                "\t",
                "<SubTexture name=\"",
                "x",
                "y",
                "width",
                "height",
                "\""
            };
            for (int i = 4; i < fileText.Length - 1; i++)
            {
                for (int j = 0; j < xmlArguments.Length; j++)
                    fileText[i] = fileText[i].Replace(xmlArguments[j], "");
                fileText[i] = fileText[i].Insert(FindChar(fileText[i], '=') - 1, ":");
                fileText[i] = fileText[i].Replace("=", "");
                fileText[i] = fileText[i].Replace("/>", "");
            }
            string[] formatedArray = new string[fileText.Length - 4];
            for (int i = 4; i < fileText.Length - 1; i++)
                formatedArray[i - 4] = fileText[i];
            return formatedArray;
        }
        private string[] GetFileText(string file)
        {
            string rawShit = string.Empty;
            using (StreamReader sr = new StreamReader(file))
                rawShit = sr.ReadToEnd();
            string[] outputArray = rawShit.Trim().Split('\n');
            for (int i = 0; i < outputArray.Length; i++)
                outputArray[i] = outputArray[i].Trim();
            return outputArray;
        }
        private int FindChar(string str, char symbol)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i] == symbol) return i;
            return 0;
        }
    }
}
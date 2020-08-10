using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GameCode.Misc
{
    public class ReadFile
    {
        /// <summary>
        /// Try to read the tsx file grabbing the points
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Vector2> GetVerticiesFromTSX(string name)
        {
            try
            {
                string filePath = @"Level\LevelFiles\" + name + ".tsx";

                XmlDocument parser = new XmlDocument();
                // load file
                parser.Load(filePath);

                List<Vector2> vPoints = new List<Vector2>();

                // get all the points
                for (int i = 0; i < parser.DocumentElement.SelectNodes("tile").Count; i++)
                {
                    var vectPoints = parser.DocumentElement.SelectNodes("tile")[i].SelectSingleNode("objectgroup").ChildNodes;

                    for (int ii = 0; ii < vectPoints.Count; ii++)
                    {
                        // set them to floats
                        var x = float.Parse(vectPoints[ii].Attributes["x"].Value);
                        var y = float.Parse(vectPoints[ii].Attributes["y"].Value);

                        // add to list vector 2
                        vPoints.Add(new Vector2(x, y));
                    }
                }

                // return
                return vPoints;
            }
            catch
            {
                // if error return null
                return null;
            }
        }

        public static List<Vector2> GetVerticiesFromTMX(string levelName)
        {
            try
            {
                string filePath = @"Level\LevelFiles\" + levelName;

                XmlDocument parser = new XmlDocument();
                // load file
                parser.Load(filePath);

                List<Vector2> vPoints = new List<Vector2>();

                // get all the points
                for (int i = 0; i < parser.DocumentElement.SelectNodes("objectgroup").Count; i++)
                {
                    var vectPoints = parser.DocumentElement.SelectNodes("objectgroup")[i].SelectNodes("object");

                    for (int ii = 0; ii < vectPoints.Count; ii++)
                    {
                        if (vectPoints[ii].Attributes["width"] == null || vectPoints[ii].Attributes["height"] == null)
                        {
                            // set them to floats
                            var x = float.Parse(vectPoints[ii].Attributes["x"].Value);
                            var y = float.Parse(vectPoints[ii].Attributes["y"].Value);
                            // add to list vector 2
                            vPoints.Add(new Vector2(x, y));
                        }
                    }
                }

                // return
                return vPoints.Distinct().ToList();
            }
            catch
            {
                // if error return null
                return null;
            }
        }
    }
}

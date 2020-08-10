using Game1.Engine.Pathfinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml;

public partial class LevelLoader
{
    private const string LevelPath = "Level/LevelFiles/";

    // Need to get tile height and width for path finding grid.
    public int levelHeight { get; private set; }
    public int levelWidth { get; private set; }

    public int tileHeight { get; private set; }
    public int tileWidth { get; private set; }

    public IGrid grid { get; private set; }

    /// <summary>
    /// Class for loading levels
    /// Create Map file in Tiled (.tmx) and load in assets found in \Engine\LevelLoader\Levels\Tiles
    /// Call requestLevel with the name of the file as a string to load in
    /// </summary>
    public LevelLoader() { }

    /// <summary>
    /// Takes the requested level, parses the file and returns a list of LevelAssets to add
    /// </summary>
    /// <param name="level">The file name of the level to load</param>
    /// <returns></returns>
    public List<LevelInfo.LevelAsset> requestLevel(string level)
    {
        return parseLevel(level);
    }

    /// <summary>
    /// Parses level file and creates a list of assets to be added
    /// </summary>
    private List<LevelInfo.LevelAsset> parseLevel(string level)
    {
        XmlDocument parser = new XmlDocument();
        parser.Load(level.Insert(0, LevelPath));

        Dictionary<int, LevelInfo.AssetInfo> assetDictionary = createAssetDictionary(parser.DocumentElement.SelectNodes("tileset"));

        tileHeight = int.Parse(parser.DocumentElement.Attributes["tileheight"].Value);
        tileWidth = int.Parse(parser.DocumentElement.Attributes["tilewidth"].Value);

        levelHeight = int.Parse(parser.DocumentElement.Attributes["height"].Value);
        levelWidth = int.Parse(parser.DocumentElement.Attributes["width"].Value);

        grid = new Grid(levelWidth, levelHeight, tileWidth, tileHeight);

        //string levelData = parser.DocumentElement.SelectNodes("layer")[0].SelectSingleNode("data").InnerText;

        var datalevel = parser.DocumentElement.SelectNodes("layer");
        List<LevelInfo.LevelAsset> levelAssetList = new List<LevelInfo.LevelAsset>();

        for (int i = 0; i < datalevel.Count; i++)
        {
            string levelData = parser.DocumentElement.SelectNodes("layer")[i].SelectSingleNode("data").InnerText;
            string[] lines = levelData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            bool main = false;

            if (datalevel[i].Attributes[1].Value.ToLower().Contains("floor"))
            {
                main = false;
            }

            if (datalevel[i].Attributes[1].Value.ToLower().Contains("main"))
            {
                main = true;
            }

            int columnNumber = 0;
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] splitLine = line.Split(new[] { "," }, StringSplitOptions.None);

                    int rowNumber = 0;
                    foreach (string asset in splitLine)
                    {
                        if (asset != "0" && !string.IsNullOrWhiteSpace(asset))
                        {
                            var levelAssests = new LevelInfo.LevelAsset(new Vector2(rowNumber * tileWidth, columnNumber * tileHeight), assetDictionary[int.Parse(asset)]);

                            if (!levelAssests.info.texture.Contains("player") && main)
                            {
                                grid.grid[rowNumber, columnNumber].Walkable = false;
                            }

                            levelAssetList.Add(levelAssests);
                        }
                        rowNumber++;
                    }
                    columnNumber++;
                }
            }
        }

        //string[] lines = levelData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        //List<LevelInfo.LevelAsset> levelAssetList = new List<LevelInfo.LevelAsset>();

        //int columnNumber = 0;
        //foreach (string line in lines)
        //{
        //    if (!string.IsNullOrWhiteSpace(line))
        //    {
        //        string[] splitLine = line.Split(new[] { "," }, StringSplitOptions.None);

        //        int rowNumber = 0;
        //        foreach (string asset in splitLine)
        //        {
        //            if (asset != "0" && !string.IsNullOrWhiteSpace(asset))
        //            {
        //                var levelAssests = new LevelInfo.LevelAsset(new Vector2(rowNumber * tileWidth, columnNumber * tileHeight), assetDictionary[int.Parse(asset)]);

        //                if (!levelAssests.info.texture.Contains("player"))
        //                {
        //                    grid.grid[rowNumber, columnNumber].Walkable = false;
        //                }

        //                levelAssetList.Add(levelAssests);
        //            }
        //            rowNumber++;
        //        }
        //        columnNumber++;
        //    }
        //}

        return levelAssetList;
    }

    /// <summary>
    /// Generates dictionairy of corresponding asset file to asset
    /// </summary>
    /// <param name="tileset">The tileset parsed from the level file</param>
    /// <returns>Dictionairy of each tileset uid and info</returns>
    private Dictionary<int, LevelInfo.AssetInfo> createAssetDictionary(XmlNodeList tileset)
    {
        Dictionary<int, LevelInfo.AssetInfo> textureDict = new Dictionary<int, LevelInfo.AssetInfo>();
        List<Vector2> vPoints;

        foreach (XmlNode node in tileset)
        {
            int uid = int.Parse(node.Attributes["firstgid"].Value);

            string asset = node.Attributes["source"].Value;

            XmlDocument parser = new XmlDocument();
            parser.Load(asset.Insert(0, LevelPath));

            var properties = parser.DocumentElement.SelectNodes("properties")[0].SelectNodes("property");

            for (int i = 0; i < properties.Count; i++)
            {
                var attributes = properties[i].Attributes;

                for (int ii = 0; ii < attributes.Count; ii++)
                {
                    if (attributes[ii].Value == "folder")
                    {
                        var folderPath = attributes["value"].Value + "/";
                        asset = parser.DocumentElement.Attributes["name"].Value.Insert(0, folderPath);
                    }
                }

            }

            //asset = parser.DocumentElement.Attributes["name"].Value.Insert(0, "Walls/");

            string propertyName = parser.DocumentElement.SelectNodes("properties")[0].SelectSingleNode("property").Attributes["name"].Value;


            vPoints = new List<Vector2>();

            try
            {
                //parser.DocumentElement.SelectNodes("tile")[0].SelectSingleNode("objectgroup").SelectSingleNode("object").SelectSingleNode("polygon").Attributes["points"].Value.Split(' ');

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
            }
            catch
            {
            }

            if (propertyName == "class")
            {
                Type type = Type.GetType("GameCode.Entities." + parser.DocumentElement.SelectNodes("properties")[0].SelectSingleNode("property").Attributes["value"].Value);

                LevelInfo.AssetInfo newAsset = new LevelInfo.AssetInfo(asset, type, vPoints);

                textureDict.Add(uid, newAsset);
            }
        }

        return textureDict;
    }

    public Vector2 GetObjectPosition(string level, string name)
    {
        XmlDocument parser = new XmlDocument();
        parser.Load(level.Insert(0, LevelPath));

        for (int i = 0; i < parser.DocumentElement.SelectNodes("objectgroup").Count; i++)
        {
            var vectPoints = parser.DocumentElement.SelectNodes("objectgroup")[i].SelectNodes("object");

            for (int ii = 0; ii < vectPoints.Count; ii++)
            {
                if (vectPoints[ii].Attributes["width"] != null || vectPoints[ii].Attributes["height"] != null)
                {
                    // set them to floats
                    var x = float.Parse(vectPoints[ii].Attributes["x"].Value);
                    var y = float.Parse(vectPoints[ii].Attributes["y"].Value);
                    // add to list vector 2
                    return new Vector2(x, y);
                }
            }
        }

        return Vector2.Zero;
    }

}
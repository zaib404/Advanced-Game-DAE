using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class LevelInfo
{
    /// <summary>
    /// Struct for a LevelAssets position and AssetInfo
    /// </summary>
    public struct LevelAsset
    {
        public LevelAsset(Vector2 pos, AssetInfo inf)
        {
            position = pos;
            info = inf;
        }

        public Vector2 position;
        public AssetInfo info;

    }

    /// <summary>
    /// Struct containing information of the assets texture and type
    /// </summary>
    public struct AssetInfo
    {
        public AssetInfo(string tex, Type ty, List<Vector2> pVerts)
        {
            texture = tex;
            type = ty;
            verts = pVerts;
        }

        public string texture;
        public Type type;
        public List<Vector2> verts;
    }

}
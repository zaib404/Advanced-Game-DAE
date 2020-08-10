using Engine.Entity;
using Game1.Engine.Pathfinding;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine
{
    public interface IEngineAPI
    {
        /// <summary>
        /// Load a game level
        /// </summary>
        /// <param name="level">The name of the level to load</param>
        /// <returns>A list of all loaded entities</returns>
        List<iEntity> LoadLevel(List<LevelInfo.LevelAsset> levelInfo);

        /// <summary>
        /// Request to create a new Instance of Entity and set up
        /// </summary>
        /// <param name="postion">The position to set the entity</param>
        /// <param name="texture">The texture of the requested entity</param>
        /// <typeparam name="T">Generic</typeparam>
        /// <returns>Reference to EntityInstance</returns>
        T LoadEntity<T>(string texture, Vector2 postion, List<Vector2> verts = default(List<Vector2>)) where T : iEntity, new();

        /// <summary>
        /// Request to create a new Instance of UI
        /// </summary>
        /// <param name="postion">The position to set the entity</param>
        /// <param name="texture">The texture of the requested entity</param>
        /// <typeparam name="T">Generic</typeparam>
        /// <returns>Reference to EntityInstance</returns>
        T LoadUI<T>(string texture, Vector2 postion) where T : iEntity, new();
        
        /// <summary>
        /// Unloads the specified entity
        /// </summary>
        /// <param name="ent">The entity to unload</param>
        void UnLoad(iEntity ent);

        void SetPathFindingGrid(IGrid mGrid, bool showGrid = false);

        void UnloadWholeLevel();
    }
}
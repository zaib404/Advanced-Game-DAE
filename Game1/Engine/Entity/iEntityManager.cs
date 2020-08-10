using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Entity
{
    /// <summary>
    /// Manages entities lifecycle
    /// </summary>
    public interface iEntityManager
    {
        /// <summary>
        /// Store a list of Entities
        /// </summary>
        List<iEntity> storeEntity { get; set; }

        /// <summary>
        /// Request to create a new Instance of Entity and set up
        /// </summary>
        /// <param name="postion">The position to set the entity</param>
        /// <param name="texture">The texture of the requested entity</param>
        /// <typeparam name="T">Generic</typeparam>
        /// <returns>Reference to EntityInstance</returns>
        T RequestInstanceAndSetup<T>(string texture, Vector2 postion, List<Vector2> verts = default(List<Vector2>)) where T : iEntity, new();

        /// <summary>
        /// Take level info and init all
        /// </summary>
        /// <param name="level">List of LevelInfo</param>
        /// <returns>Returns the initialised IEntitites</returns>
        List<iEntity> CreateLevel(List<LevelInfo.LevelAsset> level);

        /// <summary>
        /// Get Entity from their ID and name
        /// </summary>
        /// <param name="id">Unique Identifier</param>
        /// <param name="name">Unique Name</param>
        /// <returns>Entity</returns>
        iEntity GetEntity(iEntity pEntity);

        /// <summary>
        /// Termination of specfic Entity
        /// </summary>
        /// <param name="UID">Unique Identifier</param>
        /// <param name="UName">Unique Name</param>
        void Terminate(iEntity pEntity);

        void TerminateAll();
    }
}
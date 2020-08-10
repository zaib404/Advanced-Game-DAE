using Engine.Entity;
using System;
using System.Collections.Generic;

namespace Engine.Scene
{
    /// <summary>
    /// Implements database representing the state
    /// of the gameplay scene
    /// </summary>
    interface iSceneGraph
    {
        /// <summary>
        /// List of child nodes in scene
        /// </summary>
        List<iEntity> childNodes { get; set; }

        /// <summary>
        /// Add entity to scene
        /// </summary>
        /// <param name="entity">entity</param>
        void addEntity(iEntity entity);

        /// <summary>
        /// remove entity in scene
        /// </summary>
        /// <param name="entity"></param>
        void removeEntity(iEntity entity);

        /// <summary>
        /// remove entity in scene
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Unique Name</param>
        void removeEntity(Guid id, string name);

        /// <summary>
        /// Claer the whole scene
        /// </summary>
        void removeAll();

        //void addComponents();
    }
}

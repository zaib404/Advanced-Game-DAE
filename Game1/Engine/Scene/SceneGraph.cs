using Engine.Entity;
using System;
using System.Collections.Generic;

namespace Engine.Scene
{
    /// <summary>
    /// Implements database representing the state
    /// of the gameplay scene
    /// </summary>
    class SceneGraph : iSceneGraph
    {

        #region Properties

        public List<iEntity> childNodes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public SceneGraph()
        {
            Initialize();
        }

        /// <summary>
        /// Setup of initial gameplay state
        /// </summary>
        private void Initialize()
        {
            childNodes = new List<iEntity>();
        }

        /// <summary>
        /// Add entity to scene
        /// </summary>
        /// <param name="entity">Entity</param>
        public void addEntity(iEntity entity)
        {
            // if entity isnt in list then add it to list
            if (!childNodes.Contains(entity))
            {
                // add to list so its in scene
                childNodes.Add(entity);
            }
        }

        /// <summary>
        /// Remove entity from scene
        /// </summary>
        /// <param name="entity">remove entity in scene</param>
        public void removeEntity(iEntity entity)
        {
            // if it exists in the list then remove it
            if (childNodes.Contains(entity))
            {
                childNodes.Remove(entity);
            }
        }

        /// <summary>
        /// Remove entity in scene using UID and UName
        /// to identify the entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void removeEntity(Guid id, string name)
        {
            // dont think i need if statement

            // remove entity that matches id and uname
            childNodes.RemoveAll(
                x => x.UID.Equals(id) && x.UName.Equals(name));
        }

        /// <summary>
        /// Clear the whole scene
        /// </summary>
        public void removeAll()
        {
            childNodes.Clear();
        }

        #endregion
    }
}

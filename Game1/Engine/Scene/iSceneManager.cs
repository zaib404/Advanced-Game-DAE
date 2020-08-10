using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Scene
{
    /// <summary>
    /// Manages gameplay scene
    /// </summary>
    interface iSceneManager
    {
        /// <summary>
        /// Record entity has been spawned
        /// </summary>
        List<iEntity> storeEntity { get; set; }

        /// <summary>
        /// Spawn entity into scene
        /// </summary>
        /// <param name="entityInstance">Spawn Entity</param>
        void Spawn(iEntity entityInstance);

        /// <summary>
        /// Spawns a UI iEntity
        /// </summary>
        /// <param name="UI">The UI to spawn</param>
        void SpawnUI(iEntity UI);

        /// <summary>
        /// Entity Removal from scene
        /// </summary>
        /// <typeparam name="T">Dynamic</typeparam>
        /// <param name="uid">Unique ID</param>
        /// <param name="uname">Unique Name</param>
        void Remove<T>(iEntity pEntity) where T : iEntity;

        /// <summary>
        /// Retrieval of reference to spawned entity
        /// </summary>
        /// <typeparam name="T">Dynamic typing</typeparam>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>Reference to entity</returns>
        iEntity GetEntity<T>(iEntity pEntity) where T : iEntityManager;

        /// <summary>
        /// Load Resources in SceneGraph
        /// </summary>
        void LoadResources();

        /// <summary>
        /// Loads a single resource
        /// </summary>
        /// <param name="ent">The entity to load</param>
        void LoadResource(iEntity ent);

        /// <summary>
        /// The unload content ensures all content from the current scene is
        /// released before loading a new scene
        /// </summary>
        void UnloadContent();

        void Initialize(GraphicsDevice graph);


        void Update(GameTime gameTime);

        void Draw();

    }
}

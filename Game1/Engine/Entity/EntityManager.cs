using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Entity
{
    //[Serializable]
    /// <summary>
    /// Manages entities lifecycle
    /// </summary>
    public class EntityManager : iEntityManager
    {
        #region Data Members

        /// <summary>
        /// Store list of the entity names
        /// </summary>
        List<String> entityNames;

        #endregion

        #region Properties

        /// <summary>
        /// Store entity reference
        /// </summary>
        public List<iEntity> storeEntity { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityManager()
        {
            storeEntity = new List<iEntity>();
            entityNames = new List<string>();
        }


        /// <summary>
        /// Request instance and call Setup on the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="texture">The texture string</param>
        /// <param name="position">POsition of the entity</param>
        /// <returns></returns>
        public T RequestInstanceAndSetup<T>(string texture, Vector2 position, List<Vector2> verts = default(List<Vector2>)) where T : iEntity, new()
        {
            return CreateInstanceAndSetup<T>(texture, position, verts);
        }


        /// <summary>
        /// Take level info and init all
        /// </summary>
        /// <param name="level">List of LevelInfo</param>
        /// <returns>Returns the initialised IEntitites</returns>
        public List<iEntity> CreateLevel(List<LevelInfo.LevelAsset> level)
        {
            List<iEntity> returnList = new List<iEntity>();

            foreach (var asset in level)
            {
                var ent = (iEntity)Activator.CreateInstance(asset.info.type);

                if (asset.info.verts.Count > 0)
                {
                    Setup(ent, asset.info.texture, asset.position, asset.info.verts);
                }
                else
                {
                    Setup(ent, asset.info.texture, asset.position, default(List<Vector2>));
                }

                returnList.Add(ent);
            }

            return returnList;
        }

        /// <summary>
        /// Create Instance, this is where it generates a id
        /// and gives its uname
        /// </summary>
        /// <typeparam name="T">Generic</typeparam>
        private T CreateInstance<T>() where T : iEntity, new()
        {
            T requestedEntity = new T();

            storeEntity.Add(requestedEntity);

            return requestedEntity;
        }

        /// <summary>
        /// Creates the requested instance and sets it up
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="texture"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private T CreateInstanceAndSetup<T>(string texture, Vector2 pos, List<Vector2> verts) where T : iEntity, new()
        {
            var requestedEntity = CreateInstance<T>();
            Setup(requestedEntity, texture, pos, verts);

            return requestedEntity;
        }

        /// <summary>
        /// Sets up the entity
        /// </summary>
        /// <param name="entity">The entity to setup</param>
        /// <param name="texture">The texture to set for that entity</param>
        /// <param name="pos">The position of the entity</param>
        private void Setup(iEntity entity, string texture, Vector2 pos, List<Vector2> verts)
        {
            var id = Guid.NewGuid();

            entity.Setup(id, setEntityUName(entity.GetType().Name), texture, pos, verts);
        }

        /// <summary>
        /// Give entity unique name by class and number
        /// </summary>
        /// <param name="name">class name</param>
        /// <returns></returns>
        private string setEntityUName(string name)
        {
            entityNames.Add(name);
            // return name with number of how many there are in the list -1 to give accurate number.
            return name + (entityNames.Select(n => n).Where(n => n == name).Count() - 1);
        }

        /// <summary>
        /// Look through list to find the entity and return it
        /// </summary>
        /// <param name="id">Unique Identifier</param>
        /// <param name="name">Unique Name</param>
        /// <returns>Entity</returns>
        public iEntity GetEntity(iEntity entity)
        {
            try
            {
                return storeEntity.AsEnumerable().Select(e => e).
                Where(e => e.Equals(entity)).ToList()[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Termination of Entity not sure how to
        /// Maybe look for entity in list and set to null?
        /// </summary>
        /// <param name="UID">Unique Identifier</param>
        /// <param name="UName">Unique Name</param>
        public void Terminate(iEntity pEntity)
        {
            if (storeEntity.Contains(pEntity))
            {
                storeEntity.Remove(pEntity);
                entityNames.Remove(entityNames.Where(x => pEntity.UName.Contains(x)).ToList()[0]);
            }
        }

        public void TerminateAll()
        {
            storeEntity.Clear();
            entityNames.Clear();
        }

        #endregion
    }
}
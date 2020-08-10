using Engine.Camera;
using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Render
{
    public class Renderer
    {
        private List<iEntity> entityList;
        private List<iEntity> uiList;

        private Viewport defaultView = new Viewport(0, 0, EngineMain.ScreenWidth, EngineMain.ScreenHeight);

        private SpriteBatch spriteBatch;
        private GraphicsDevice graphDevice;

        private CameraManager cameraMan;

        public Renderer()
        {
            entityList = new List<iEntity>();
            uiList = new List<iEntity>();
            cameraMan = new CameraManager();
        }

        /// <summary>
        /// Initialise the renderer and spawn camera manager
        /// </summary>
        public void Init(GraphicsDevice graph)
        {
            graphDevice = graph;
            spriteBatch = new SpriteBatch(graphDevice);
        }

        /// <summary>
        /// Add an entity to render list
        /// </summary>
        /// <param name="ent">The entity to add</param>
        public void addEntity(iEntity ent)
        {
            entityList.Add(ent);

            if (ent is ICameraSubject)
            {
                cameraMan.RequestCamera(ent);
                cameraMan.AddCameras();
            }
        }

        /// <summary>
        /// Add UI element to be rendered
        /// </summary>
        /// <param name="ui">The UI element to draw</param>
        public void addUI(iEntity ui)
        {
            uiList.Add(ui);
        }

        /// <summary>
        /// Draws all entities to be rendered
        /// </summary>
        public void Draw()
        {
            foreach (var cam in cameraMan.Update())
            {
                graphDevice.Viewport = cam.viewPort;

                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, cam.transform);
                drawEntities(entityList);
                spriteBatch.End();
            }

            graphDevice.Viewport = defaultView;

            spriteBatch.Begin();
            drawEntities(uiList);
            spriteBatch.End();

        }

        /// <summary>
        /// Takes a list of entities to draw with SpriteBatch
        /// </summary>
        /// <param name="drawList"></param>
        private void drawEntities(List<iEntity> drawList)
        {
            foreach (var entity in drawList)
            {
                spriteBatch.Draw(entity.Texture, entity.Position, null, Color.White * entity.Transparency, entity.Rotation, new Vector2(0, 0), 1, SpriteEffects.None, entity.DrawPriority);
            }
        }


        //***************************************** New Code written for Post Production *****************************************//

        /// <summary>
        /// Entity Removal
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        public void Remove(iEntity entity)
        {
            // remove entity or ui
            if (entityList.Contains(entity))
            {
                entityList.Remove(entity);
            }

            if (uiList.Contains(entity))
            {
                uiList.Remove(entity);
            }

            if (entity is ICameraSubject)
            {
                cameraMan.RequestRemoveCamera(entity);
                cameraMan.AddCameras();
            }
        }

        /// <summary>
        /// Empty the list
        /// </summary>
        public void RemoveAll()
        {
            entityList.Clear();
            uiList.Clear();
            cameraMan.RequestRemoveAllCamera();
        }

    }

}

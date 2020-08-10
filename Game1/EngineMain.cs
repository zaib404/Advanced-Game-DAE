using Engine.Entity;
using Engine.Scene;
using Game1.Engine.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Engine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class EngineMain : Game, IEngineAPI
    {
        GraphicsDeviceManager graphics;
        iSceneManager sceneManager;
        iEntityManager entityManager;

        public static int ScreenWidth = 1600, ScreenHeight = 900;
        bool paused = false;

        IPathFinding pathFind;

        public EngineMain()
        {
            #region Setup
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/Resources";

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            this.IsMouseVisible = true;

            // Setting screen to middle
            Window.Position = new Point
                ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
                (graphics.PreferredBackBufferWidth / 2),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
                (graphics.PreferredBackBufferHeight / 2));

            #endregion

            sceneManager = new SceneManager(Content);
            entityManager = new EntityManager();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        public List<iEntity> LoadLevel(List<LevelInfo.LevelAsset> levelInfo)
        {
            var entities = entityManager.CreateLevel(levelInfo);
            foreach (var ent in entities)
            {
                if (GraphicsDevice != null)
                {
                    sceneManager.LoadResource(ent);
                }

                sceneManager.Spawn(ent);

            }

            return entities;
        }

        public void SetPathFindingGrid(IGrid pGrid, bool showGrid)
        {
            pathFind = new PathFinding(pGrid);

            if (showGrid)
            {
                for (int row = 0; row < pGrid.grid.GetLength(0); row++)
                {
                    for (int column = 0; column < pGrid.grid.GetLength(1); column++)
                    {
                        sceneManager.Spawn((iEntity)pathFind.mGrid.grid[row, column]);

                        if (GraphicsDevice != null)
                        {
                            sceneManager.LoadResource((iEntity)pathFind.mGrid.grid[row, column]);
                        }
                    }
                }
            }
        }

        public T LoadEntity<T>(string texture, Vector2 position, List<Vector2> verts = default(List<Vector2>)) where T : iEntity, new()
        {
            var ent = entityManager.RequestInstanceAndSetup<T>(texture, position, verts);
            sceneManager.Spawn(ent);

            if (GraphicsDevice != null)
            {
                sceneManager.LoadResource(ent);
            }

            return ent;
        }

        public T LoadUI<T>(string texture, Vector2 position) where T : iEntity, new()
        {
            var ui = entityManager.RequestInstanceAndSetup<T>(texture, position);
            sceneManager.SpawnUI(ui);

            if (GraphicsDevice != null)
            {
                sceneManager.LoadResource(ui);
            }

            return ui;
        }

        public void UnLoad(iEntity ent)
        {
            sceneManager.Remove<iEntity>(ent);
            entityManager.Terminate(ent);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            sceneManager.Initialize(GraphicsDevice);
            sceneManager.LoadResources();
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            sceneManager.UnloadContent();
            entityManager.TerminateAll();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                paused = !paused;
            }

            if (!paused)
            {
                sceneManager.Update(gameTime);
            }


            #region FPS counter
            // Code for displaying FPS in output
            //private FrameCounter frameCounter = new FrameCounter();
            //var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //frameCounter.Update(deltaTime);
            //var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            //Console.WriteLine("Current fps - " + fps);
            #endregion

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            sceneManager.Draw();

            base.Draw(gameTime);
        }

        public void UnloadWholeLevel()
        {
            sceneManager.UnloadContent();
            entityManager.TerminateAll();
        }

    }
}

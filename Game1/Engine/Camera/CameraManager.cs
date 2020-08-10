using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Camera
{
    class CameraManager
    {
        #region Makes me sick

        private List<Vector2> viewPortPos1 = new List<Vector2>()
        {
            new Vector2(0, 0)
        };

        private List<Vector2> viewPortPos2 = new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(0,450)
        };

        private List<Vector2> viewPortPos3 = new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(800,0),
            new Vector2(400, 450)
        };

        private List<Vector2> viewPortPos4 = new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(800,0),
            new Vector2(0, 450),
            new Vector2(800, 450)
        };

        #endregion


        public static List<iEntity> subjectList = new List<iEntity>();
        private List<Camera> cameraList = new List<Camera>();

        /// <summary>
        /// Struct for storing cameras Viewport and transform matrix
        /// </summary>
        public struct CameraView
        {
            public CameraView(Viewport view, Matrix tran)
            {
                viewPort = view;
                transform = tran;
            }

            public Viewport viewPort;
            public Matrix transform;
        }


        public CameraManager()
        {
        }


        /// <summary>
        /// Request camera and pass entity to follow
        /// </summary>
        /// <param name="entity">The entity to follow</param>
        public void RequestCamera(iEntity entity)
        {
            subjectList.Add(entity);
        }

        /// <summary>
        /// Adds camera for each entity that has requested one
        /// </summary>
        public void AddCameras()
        {
            #region Makes me sick x2
            List<Vector2> viewPosList;
            if (subjectList.Count <= 1)
            {
                viewPosList = viewPortPos1;
            }
            else if (subjectList.Count == 2)
            {
                viewPosList = viewPortPos2;
            }
            else if(subjectList.Count == 3)
            {
                viewPosList = viewPortPos3;
            }
            else
            {
                viewPosList = viewPortPos4;
            }
            #endregion

            cameraList.Clear();

            var resolution = new Vector2(EngineMain.ScreenWidth, EngineMain.ScreenHeight);

            int subjectCount = 0;
            foreach(var subject in subjectList)
            {
                var camera = new Camera(subject, new Rectangle((int)viewPosList[subjectCount].X, (int)viewPosList[subjectCount].Y, 
                                        (int)resolution.X / (subjectList.Count <= 2 ? 1 : 2), (int)resolution.Y / (subjectList.Count <= 1 ? 1 : 2)));
                cameraList.Add(camera);
                subjectCount++;
            }
        }

        /// <summary>
        /// Updates each of the cameras CameraView and returns a list
        /// </summary>
        /// <returns>List of CameraViews</returns>
        public List<CameraView> Update()
        {
            List<CameraView> camView = new List<CameraView>();
            foreach(var camera in cameraList)
            {
                camera.Update();
                camView.Add(new CameraView(camera.viewPort, camera.transform));
            }

            return camView;
        }


        //***************************************** New Code written for Post Production *****************************************//
        public void RequestRemoveCamera(iEntity entity)
        {
            subjectList.Remove(entity);
        }

        public void RequestRemoveAllCamera()
        {
            subjectList.Clear();
        }

    }
}

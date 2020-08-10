using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Camera
{
    public class Camera
    {
        private iEntity subject;
        public Matrix transform;
        public Viewport viewPort;

        public Camera(iEntity sub, Rectangle cameraInfo)
        {
            subject = sub;
            viewPort = new Viewport(cameraInfo);
        }

        /// <summary>
        /// Work out the transform required for the camera based on the cameras entity position
        /// </summary>
        public void Update()
        {
            Vector2 centre = new Vector2(subject.Position.X - viewPort.Width / 2, subject.Position.Y - viewPort.Height / 2);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }
        
    }
}

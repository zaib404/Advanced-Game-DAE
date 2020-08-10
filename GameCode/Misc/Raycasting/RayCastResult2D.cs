using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Engine.Raycasting
{
    /// <summary>
    /// By 'Laurent Cozic'
    /// https://www.codeproject.com/Articles/15604/Ray-casting-in-a-2D-tile-based-environment
    /// Slight Modifications made to fit engine
    /// </summary>
    // Class returned by the RayCast() method
    public class RayCastingResult2D
    {
        // Does the ray collide with the environment?
        private bool doCollide;
        // And if so, at which position?
        private Vector2 position;

        public bool DoCollide
        {
            get { return doCollide; }
            set { doCollide = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }


}

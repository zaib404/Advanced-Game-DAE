using Engine.Shape;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Misc
{
    class MathsHelper
    {
        public struct Normal
        {
            public Normal(Vector2 pPoint1, Vector2 pPoint2)
            {
                point1 = pPoint1;
                point2 = pPoint2;
            }

            public Vector2 point1;
            public Vector2 point2;
        }


        //Gets the dot product of 2 vectors
        public static float DotProduct(Vector2 point1, Vector2 point2)
        {
            return (point1.X * point2.X) + (point1.Y * point2.Y);
        }
        
        /// <summary>
        /// Gets the normal of 2 vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Normal NormalOfVector(Vector2 point1, Vector2 point2)
        {
            float dx = point2.X - point1.X;
            float dy = point2.Y - point1.Y;

            return new Normal(new Vector2(-dy, dx), new Vector2(dy, -dx));
        }
        
        /// <summary>
        /// Tests if a shape has 4 vertices and if the distance between 3 of the points is equal to the width and
        /// height of the original bounding box
        /// </summary>
        /// <param name="shape">The IShape to test</param>
        /// <returns>boolean for whether the shape is the same as its bounding box</returns>
        public static bool isSameAsBoundingBox(IShape shape)
        {
            if(shape.GetVertices().Count == 0)
            {
                return true;
            }

            if (shape.GetVertices().Count == 4)
            {
                Vector2 pointA = shape.GetVertices()[0];
                Vector2 pointB = shape.GetVertices()[1];
                Vector2 pointC = shape.GetVertices()[2];
                Vector2 pointD = shape.GetVertices()[3];

                float ABDistance = (pointB - pointA).X;
                float BCDistance = (pointC - pointA).Y;
                float CDDistance = (pointC - pointD).X;

                return ABDistance == shape.GetBoundingBox().Width
                    && BCDistance == shape.GetBoundingBox().Height
                    && CDDistance == shape.GetBoundingBox().Width;
            }

            return false;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Engine.Misc.MathsHelper;

namespace Engine.Collision
{
    /// <summary>
    /// Class with methods for seperating axis theorem
    /// </summary>
    class SAT
    {
        /// <summary>
        /// Checks if to list of vertices are overlapping
        /// </summary>
        /// <param name="shape1Verts"></param>
        /// <param name="shape2Verts"></param>
        /// <returns>True if there is any overlap</returns>
        public static bool Overlapping(List<Vector2> shape1Verts, List<Vector2> shape2Verts)
        {
            //We have to check the vertex between the ifrst and the last outside the loop
            Vector2 firstPoint = shape1Verts.First();
            Vector2 lastPoint = shape1Verts.Last();

            Normal norm = NormalOfVector(firstPoint, lastPoint);
            List<Vector2> shape1Projections = CalculateProjections(norm, shape1Verts);
            List<Vector2> shape2Projections = CalculateProjections(norm, shape2Verts);

            var orderedShape1Proj = shape1Projections.OrderBy(proj => proj.X + proj.Y).ToList();
            var orderedShape2Proj = shape2Projections.OrderBy(proj => proj.X + proj.Y).ToList();

            if ((orderedShape1Proj.Last().X + orderedShape1Proj.Last().Y) < (orderedShape2Proj.First().X + orderedShape2Proj.First().Y)
                || (orderedShape1Proj.First().X + orderedShape1Proj.First().Y) > (orderedShape2Proj.Last().X + orderedShape2Proj.Last().Y))
            {
                return false;
            }

            for (int i = 0; i <= shape1Verts.Count - 2; i++)
            {
                norm = NormalOfVector(shape1Verts[i], shape1Verts[i + 1]);

                shape1Projections = CalculateProjections(norm, shape1Verts);
                shape2Projections = CalculateProjections(norm, shape2Verts);

                orderedShape1Proj = shape1Projections.OrderBy(proj => proj.X + proj.Y).ToList();
                orderedShape2Proj = shape2Projections.OrderBy(proj => proj.X + proj.Y).ToList();

                if ((orderedShape1Proj.Last().X + orderedShape1Proj.Last().Y) < (orderedShape2Proj.First().X + orderedShape2Proj.First().Y)
                    || (orderedShape1Proj.First().X + orderedShape1Proj.First().Y) > (orderedShape2Proj.Last().X + orderedShape2Proj.Last().Y))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Calculates the projections for a list of vertices against a normal
        /// </summary>
        /// <param name="norm"></param>
        /// <param name="verts"></param>
        /// <returns>List of projections</returns>
        private static List<Vector2> CalculateProjections(Normal norm, List<Vector2> verts)
        {
            List<Vector2> projectionList = new List<Vector2>();
            foreach (Vector2 vert in verts)
            {
                projectionList.Add(ProjectionEquation(vert, norm.point1));
            }

            return projectionList;
        }

        /// <summary>
        /// Projection equation for SAT
        /// ( CollisionAxis . VertexVector / CollisionAxis . CollisionAxis ) CollisionAxis
        /// </summary>
        /// <param name="vertexVector"></param>
        /// <param name="collisionAxisVector"></param>
        /// <returns>Returns the projection</returns>
        private static Vector2 ProjectionEquation(Vector2 vertexVector, Vector2 collisionAxisVector)
        {
            float vertexToCollisionAxisDot = DotProduct(collisionAxisVector, vertexVector);
            float collisionAxisDot = DotProduct(collisionAxisVector, collisionAxisVector);


            float dividedDots = vertexToCollisionAxisDot / collisionAxisDot;

            Vector2 projection = dividedDots * collisionAxisVector;

            return projection;
        }


        //***************************************** New Code written for Post Production *****************************************//

        /// <summary>
        /// Followed tutorial by Laurent Cozic
        /// https://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection
        /// </summary>
        /// <param name="shape1Verts"></param>
        /// <param name="shape2Verts"></param>
        /// <returns></returns>
        public static Vector2 Overlapping2(List<Vector2> shape1Verts, List<Vector2> shape2Verts)
        {
            Vector2 MTV = new Vector2();
            float minDistance = float.PositiveInfinity;
            Vector2 smallest;

            //We have to check the vertex between the ifrst and the last outside the loop
            Vector2 firstPoint = shape1Verts.First();
            Vector2 lastPoint = shape1Verts.Last();

            Normal norm = NormalOfVector(firstPoint, lastPoint);
            List<Vector2> shape1Projections = CalculateProjections(norm, shape1Verts);
            List<Vector2> shape2Projections = CalculateProjections(norm, shape2Verts);

            var orderedShape1Proj = shape1Projections.OrderBy(proj => proj.X + proj.Y).ToList();
            var orderedShape2Proj = shape2Projections.OrderBy(proj => proj.X + proj.Y).ToList();

            if ((orderedShape1Proj.Last().X + orderedShape1Proj.Last().Y) < (orderedShape2Proj.First().X + orderedShape2Proj.First().Y)
                || (orderedShape1Proj.First().X + orderedShape1Proj.First().Y) > (orderedShape2Proj.Last().X + orderedShape2Proj.Last().Y))
            {
                return Vector2.Zero;
            }

            for (int i = 0; i <= shape1Verts.Count - 2; i++)
            {
                norm = NormalOfVector(shape1Verts[i], shape1Verts[i + 1]);

                shape1Projections = CalculateProjections(norm, shape1Verts);
                shape2Projections = CalculateProjections(norm, shape2Verts);

                orderedShape1Proj = shape1Projections.OrderBy(proj => proj.X + proj.Y).ToList();
                orderedShape2Proj = shape2Projections.OrderBy(proj => proj.X + proj.Y).ToList();

                if ((orderedShape1Proj.Last().X + orderedShape1Proj.Last().Y) < (orderedShape2Proj.First().X + orderedShape2Proj.First().Y)
                    || (orderedShape1Proj.First().X + orderedShape1Proj.First().Y) > (orderedShape2Proj.Last().X + orderedShape2Proj.Last().Y))
                {
                    return Vector2.Zero;
                }
                else
                {
                    float minA = orderedShape1Proj.First().X + orderedShape1Proj.First().Y;
                    float maxA = orderedShape1Proj.Last().X + orderedShape1Proj.Last().Y;
                    float minB = orderedShape2Proj.First().X + orderedShape2Proj.First().Y;
                    float maxB = orderedShape2Proj.Last().X + orderedShape2Proj.Last().Y;

                    float overlapDistance = GetOverlap(minA, maxA, minB, maxB);
                    overlapDistance = Math.Abs(overlapDistance);

                    if (overlapDistance < minDistance)
                    {
                        minDistance = overlapDistance;
                        smallest = orderedShape1Proj[i];

                        Vector2 check = orderedShape1Proj[i] - orderedShape2Proj[i];
                        if (check.X < 0 || check.Y < 0)
                        {
                            smallest = -smallest;
                        }

                        smallest.Normalize();

                        MTV = smallest * overlapDistance;
                    }
                }

            }

            return MTV;
        }

        public static float GetOverlap(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
    }
}

using Game1.Engine.Pathfinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Engine.Raycasting
{
    /// <summary>
    /// By 'Laurent Cozic'
    /// https://www.codeproject.com/Articles/15604/Ray-casting-in-a-2D-tile-based-environment
    /// Slight Modifications made to fit engine
    /// </summary>
    public static class RayCast2D
    {
        public static RayCastingResult2D RayCast(Vector2 position, Vector2 direction, float rayLength, IGrid pGrid)
        {
            RayCastingResult2D result = new RayCastingResult2D();
            result.DoCollide = false;

            if (rayLength == 0)
            {
                var node = pGrid.ConvertWorldPosToGrid(position);
                result.DoCollide = node.Walkable ? true : false;
                result.Position = position;
                return result;
            }

            direction.Normalize();
            // Get the list of points from the Bresenham algorithm
            List<Point> rayLine = BresenhamLine(position.ToPoint(), position.ToPoint() + (direction * rayLength).ToPoint());

            if (rayLine == null)
            {
                var node = pGrid.ConvertWorldPosToGrid(position);
                result.DoCollide = node.Walkable ? true : false;
                result.Position = position;
                return result;
            }

            if (rayLine.Count > 0)
            {
                int rayPointIndex = 0;

                if (rayLine[0] != position.ToPoint())
                {
                    rayPointIndex = rayLine.Count - 1;
                }

                // Loop through all the points starting from "position"
                while (true)
                {
                    Point rayPoint = rayLine[rayPointIndex];
                    var node = pGrid.ConvertWorldPosToGrid(new Vector2(rayPoint.X, rayPoint.Y));

                    if (node == null)
                    {
                        result.DoCollide = false;
                        result.Position = position;
                        return result;
                    }

                    if (!node.Walkable)
                    {
                        result.Position = rayPoint.ToVector2();
                        result.DoCollide = true;
                        break;
                    }
                    if (rayLine[0] != position.ToPoint())
                    {
                        rayPointIndex--;
                        if (rayPointIndex < 0) break;
                    }
                    else
                    {
                        rayPointIndex++;
                        if (rayPointIndex >= rayLine.Count) break;
                    }
                }

            }
            return result;
        }

        // Returns the list of points from p0 to p1 
        static List<Point> BresenhamLine(Point p0, Point p1)
        {
            return BresenhamLine(p0.X, p0.Y, p1.X, p1.Y);
        }

        // Returns the list of points from (x0, y0) to (x1, y1)
        static List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            // Optimization: it would be preferable to calculate in
            // advance the size of "result" and to use a fixed-size array
            // instead of a list.
            List<Point> result = new List<Point>();

            try
            {
                bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
                if (steep)
                {
                    Swap(ref x0, ref y0);
                    Swap(ref x1, ref y1);
                }
                if (x0 > x1)
                {
                    Swap(ref x0, ref x1);
                    Swap(ref y0, ref y1);
                }

                int deltax = x1 - x0;
                int deltay = Math.Abs(y1 - y0);
                int error = 0;
                int ystep;
                int y = y0;
                if (y0 < y1) ystep = 1; else ystep = -1;
                for (int x = x0; x <= x1; x++)
                {
                    if (steep) result.Add(new Point(y, x)); else result.Add(new Point(x, y));
                    error += deltay;
                    if (2 * error >= deltax)
                    {
                        y += ystep;
                        error -= deltax;
                    }
                }
            }
            catch
            {
                return null;
            }

            

            return result;
        }

        // Swap the values of A and B
        static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }
}

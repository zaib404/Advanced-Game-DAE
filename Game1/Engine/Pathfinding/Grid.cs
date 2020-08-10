using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1.Engine.Pathfinding
{
    public class Grid : IGrid
    {
        public INode[,] grid { get; private set; }

        // get access to tilesize and width
        public float tileSizeWidth { get; private set; }
        public float tileSizeHeight { get; private set; }

        public Grid(int pMapWidth, int pMapHeight, float pTileSizeWidth, float pTileSizeHeight)
        {
            // initaliz grid node
            grid = new Node[pMapWidth, pMapHeight];
            tileSizeWidth = pTileSizeWidth;
            tileSizeHeight = pTileSizeHeight;

            CreateGrid();
        }

        /// <summary>
        /// Create the 2D array Grid
        /// </summary>
        void CreateGrid()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    // create node pass the world pos and grid pos
                    grid[x, y] = new Node(new Vector2(x * tileSizeWidth, y * tileSizeHeight), new Vector2(x, y));
                }
            }
        }

        /// <summary>
        /// Gets node postion in grid
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public INode GetNodePosition(Vector2 pPos)
        {
            // Checking to see its not out of grid.
            if (pPos.X >= 0 && pPos.Y >= 0 && pPos.X < grid.GetLength(0) && pPos.Y < grid.GetLength(1))
            {
                return grid[(int)pPos.X, (int)pPos.Y];
            }
            return null;
        }

        /// <summary>
        /// Get the neighbour node of the node passed in
        /// </summary>
        /// <param name="pNode">current node</param>
        /// <returns></returns>
        public IList<INode> GetNeighbourNodes(INode pNode)
        {
            IList<INode> neighbour = new List<INode>();

            neighbour.Add(isReal((int)pNode.gridPos.X + 1, (int)pNode.gridPos.Y)); //1                          
            neighbour.Add(isReal((int)pNode.gridPos.X + 1, (int)pNode.gridPos.Y + 1)); //2                                  
            neighbour.Add(isReal((int)pNode.gridPos.X, (int)pNode.gridPos.Y + 1)); //3                                  
            neighbour.Add(isReal((int)pNode.gridPos.X - 1, (int)pNode.gridPos.Y + 1)); //4                                  
            neighbour.Add(isReal((int)pNode.gridPos.X - 1, (int)pNode.gridPos.Y)); //5                                  
            neighbour.Add(isReal((int)pNode.gridPos.X - 1, (int)pNode.gridPos.Y - 1)); //6                                  
            neighbour.Add(isReal((int)pNode.gridPos.X, (int)pNode.gridPos.Y - 1)); //7                          
            neighbour.Add(isReal((int)pNode.gridPos.X + 1, (int)pNode.gridPos.Y - 1)); //8

            IList<INode> actualList = neighbour.Where(x => x != null && x.Walkable).ToList();

            return actualList;
        }

        /// <summary>
        /// Make sure the node grid is in the grid and not out
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public INode isReal(int row, int column)
        {
            if (row >= 0 && row < grid.GetLength(0) && column >= 0 && column < grid.GetLength(1))
            {
                return grid[row, column];
            }

            return null;
        }

        //***************************************** New Code written for Post Production *****************************************//

        /// <summary>
        /// Set areas of node walkable or not walkable
        /// </summary>
        /// <param name="pPos"></param>
        /// <param name="pWalkable"></param>
        public void SetWalkable(Vector2 pPos, bool pWalkable)
        {
            // Convert node to grid coords
            var nodePos = new Vector2((int)Math.Round(pPos.X / tileSizeWidth), (int)Math.Round(pPos.Y / tileSizeHeight));

            // Get Node
            var node = GetNodePosition(nodePos);
            // Set node walkable
            node.Walkable = pWalkable;
        }

        public INode ConvertWorldPosToGrid(Vector2 pPos)
        {
            var nodePos = new Vector2((int)Math.Round(pPos.X / tileSizeWidth), (int)Math.Round(pPos.Y / tileSizeHeight));

            return GetNodePosition(nodePos);
        }
    }
}

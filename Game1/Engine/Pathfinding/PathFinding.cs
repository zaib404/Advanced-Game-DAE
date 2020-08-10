using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1.Engine.Pathfinding
{
    public class PathFinding : IPathFinding
    {
        // get reference to grid
        public IGrid mGrid { get; }

        // This is the ring that expands from the start node
        // considering to find the shortest path
        Queue<INode> frontier;
        // keeps a list of closed nodes that dont have to be visted again
        IList<INode> closeNodes;

        // movement cost
        const int straightCost = 10;
        const int diagonalCost = 14;

        public PathFinding(IGrid pGrid)
        {
            mGrid = pGrid;
            frontier = new Queue<INode>();
            closeNodes = new List<INode>();
        }

        /// <summary>
        /// Getting the grid path from start node to target node
        /// </summary>
        /// <param name="pStartPos"> start grid position</param>
        /// <param name="pTargetPos">target postion</param>
        /// <returns></returns>
        public IList<Vector2> FindPath(Vector2 pStartPos, Vector2 pTargetPos)
        {
            // if the same
            if (pStartPos == pTargetPos)
            {
                return new List<Vector2>();
            }

            // Get start node grid position
            INode startNode = mGrid.GetNodePosition(pStartPos);
            // Get target node grid position
            INode targetNode = mGrid.GetNodePosition(pTargetPos);
            // The current node is the start node
            INode currentNode = null;

            // keep track of the 'ring'
            frontier.Clear();
            // add it to the queue
            closeNodes.Clear();
            // add starnode to frontier
            frontier.Enqueue(startNode);

            // While frontier not empty
            while (frontier.Count > 0)
            {
                // get current node
                currentNode = frontier.Dequeue();
                // add to close list
                closeNodes.Add(currentNode);

                // if the current has reached target
                if (currentNode == targetNode)
                {
                    return GetFinalPath(startNode, targetNode);
                }

                // if current node is null
                if (currentNode == null)
                {
                    break;
                }

                // get each neighbour of each current node
                foreach (var neighbour in mGrid.GetNeighbourNodes(currentNode))
                {
                    // if not in closed
                    if (!closeNodes.Contains(neighbour))
                    {
                        // get the H cost 
                        startNode.HCost = EstimateHCost(startNode, targetNode);
                        // get the movement cost from current node
                        int movementCost = currentNode.GCost + EstimateHCost(currentNode, neighbour);

                        // if movementCost is less than neighbour gcost
                        if (movementCost < neighbour.GCost || !frontier.Contains(neighbour))
                        {
                            // compute score
                            neighbour.GCost = movementCost;
                            neighbour.HCost = EstimateHCost(neighbour, targetNode);
                            neighbour.Parent = currentNode;

                            if (!frontier.Contains(neighbour))
                            {
                                // add to frontier
                                frontier.Enqueue(neighbour);
                            }
                        }
                    }
                    else
                    {
                        closeNodes.Add(neighbour);
                    }
                }
            }

            // if nothing return empty
            return new List<Vector2>();
        }

        /// <summary>
        /// Convert the world coordinates to grid cords
        /// </summary>
        /// <param name="pStartWorldPos"> startNode wolrd postion</param>
        /// <param name="pTargetWorldPos">target goal end postion</param>
        /// <returns></returns>
        public IList<Vector2> FindPathWorld(Vector2 pStartWorldPos, Vector2 pTargetWorldPos)
        {
            // Convert node to grid coords
            var roundedNode = new Vector2((int)Math.Round(pStartWorldPos.X / mGrid.tileSizeWidth), (int)Math.Round(pStartWorldPos.Y / mGrid.tileSizeHeight));
            var roundedNode2 = new Vector2((int)Math.Round(pTargetWorldPos.X / mGrid.tileSizeWidth), (int)Math.Round(pTargetWorldPos.Y / mGrid.tileSizeHeight));

            // return FindPath
            return FindPath(roundedNode, roundedNode2);

        }

        /// <summary>
        /// Tracing the path backwards
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="targetNode"></param>
        private IList<Vector2> GetFinalPath(INode startNode, INode targetNode)
        {
            IList<Vector2> path = new List<Vector2>();

            INode currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            return path.Reverse().ToList();
        }

        /// <summary>
        /// Get the estimate H cost of the nodes
        /// </summary>
        /// <param name="pStartNode"></param>
        /// <param name="pTargetNode"></param>
        /// <returns></returns>
        public int EstimateHCost(INode pStartNode, INode pTargetNode)
        {
            int distanceX = (int)Math.Abs(pStartNode.gridPos.X - pTargetNode.gridPos.X);
            int distanceY = (int)Math.Abs(pStartNode.gridPos.Y - pTargetNode.gridPos.Y);

            if (distanceX > distanceY)
            {
                return diagonalCost * distanceY + straightCost * (distanceX - distanceY);
            }

            return diagonalCost * distanceX + straightCost * (distanceY - distanceX);
        }

    }
}
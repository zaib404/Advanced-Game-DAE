using Engine.Entity;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Engine.Pathfinding
{
    public class Node : Entity, INode
    {
        public INode Parent { get; set; }

        public IList<INode> Neighbours { get; set; } = null;

        public bool Visited { get; set; } = false;

        public bool Diagonal { get; set; }

        public bool Walkable { get; set; } = true;

        /// <summary>
        /// Movement cost from start node to current node
        /// </summary>
        public int GCost { get; set; }

        /// <summary>
        /// Estimated movement cost from current sqaure to destination point
        /// </summary>
        public int HCost { get; set; }

        /// <summary>
        /// Total Cost
        /// </summary>
        public int FCost => GCost + HCost;

        public Vector2 gridPos { get; set; }

        public Node(Vector2 pPosition, Vector2 pGridPos)
        {
            //Walkable = pWalkable;
            Position = pPosition;
            TextureString = "Walls/node";
            //Transparency = 0.1f;
            DrawPriority = 10;
            gridPos = pGridPos;
        }

    }
}

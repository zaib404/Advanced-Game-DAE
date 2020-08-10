using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Engine.Pathfinding
{
    public interface INode
    {
        INode Parent { get; set; }
        IList<INode> Neighbours { get; set; }
        bool Visited { get; set; }
        bool Diagonal { get; set; }
        bool Walkable { get; set; }
        Vector2 Position { get; set; }  
        Vector2 gridPos { get; set; }

        /// <summary>
        /// Movement cost from start node to current node
        /// </summary>
        int GCost { get; set; }

        /// <summary>
        /// Estimated movement cost from current sqaure to destination point
        /// </summary>
        int HCost { get; set; }

        /// <summary>
        /// Total Cost
        /// </summary>
        int FCost { get; }
    }
}

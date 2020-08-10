using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Engine.Pathfinding
{
    public interface IGrid
    {
        INode[,] grid { get; }
        INode GetNodePosition(Vector2 pPos);
        IList<INode> GetNeighbourNodes(INode pNode);
        float tileSizeWidth { get; }
        float tileSizeHeight { get; }

        //***************************************** New Code written for Post Production *****************************************//
        void SetWalkable(Vector2 pPos, bool pWalkable);
        INode ConvertWorldPosToGrid(Vector2 pPos);
    }
}

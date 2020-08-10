using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Engine.Pathfinding
{
    public interface IPathFinding
    {
        IGrid mGrid { get; }

        IList<Vector2> FindPath(Vector2 pStartPos, Vector2 pTargetPos);

        IList<Vector2> FindPathWorld(Vector2 pStartWorldPos, Vector2 pTargetWorldPos);
    }
}

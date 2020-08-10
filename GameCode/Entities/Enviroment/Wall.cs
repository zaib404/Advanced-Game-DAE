using Engine.Collision;
using Engine.Entity;

namespace GameCode.Entities
{
    class Wall : GameEntity, iCollidable
    {
        public Wall()
        {
            DrawPriority = 1;
        }
    }
}

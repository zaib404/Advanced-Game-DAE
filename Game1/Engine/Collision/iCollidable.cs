using Microsoft.Xna.Framework;

namespace Engine.Collision
{
    public interface iCollidable
    {
        Rectangle HitBox { get; }
    }
}
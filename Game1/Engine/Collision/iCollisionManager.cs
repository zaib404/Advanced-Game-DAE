using Engine.Entity;
using Engine.Shape;
using Microsoft.Xna.Framework;

namespace Engine.Collision
{
    public delegate void CollisionEvent(Vector2 MTV, iEntity player, iEntity gameObject);

    public interface iCollisionManager
    {

        //void UpdateCollidableList(List<iEntity> entList);
        //void addCollidables(List<iEntity> entList);
        //void addCollidable(iEntity ent);
        //void CheckCollision();

        void AddCollidable(IShape shape);

        //void Update();

        //***************************************** New Code written for Post Production *****************************************//

        void RemoveCollidable(IShape collidable);

        void RemoveAllCollidable();

        event CollisionEvent OnCollision;
    }
}
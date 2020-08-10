using Engine.Entity;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Shape
{
    public interface IShape
    {
        List<Vector2> GetVertices();
        Vector2 GetPosition();
        Rectangle GetBoundingBox();
        bool IsCollisionListener();

        //***************************************** New Code written for Post Production *****************************************//
        /// <summary>
        /// 'IsTrigger' must be set to true, for this to run
        /// On colliding with object the enter stage
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameObject"></param>
        void OnTriggerEnter(iEntity gameObject);
        void OnTriggerStay(iEntity gameObject);
        void OnTriggerExit(iEntity gameObject);
        bool IsTrigger { get; }
        bool IsTriggerWithAutoCollision { get; }

    }
}

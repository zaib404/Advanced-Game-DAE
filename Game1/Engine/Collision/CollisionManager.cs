using Engine.Entity;
using Engine.Managers;
using Engine.Shape;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Engine.Collision.SAT;

namespace Engine.Collision
{
    public class CollisionManager : iCollisionManager, iManager
    {
        #region Production Code
        private List<IShape> collidableList = new List<IShape>();
        private List<IShape> collisionListeners = new List<IShape>();
        private List<IShape> PotentialCollisions = new List<IShape>();
        private QuadTree qTree = new QuadTree(1, new Rectangle(0, 0, EngineMain.ScreenWidth, EngineMain.ScreenHeight));
        #endregion

        #region Post Production Code
        List<IShape> collisions = new List<IShape>();

        public event CollisionEvent OnCollision;

        #endregion

        public void AddCollidable(IShape collidable)
        {
            if (collidable.IsCollisionListener())
            {
                collisionListeners.Add(collidable);
                //return;
            }

            collidableList.Add(collidable);
            #region Post Production Code
            if (OnCollision == null)
            {
                OnCollision += CollisionManager_OnCollision;
            }
            #endregion
        }

        private void AddToQuadTree()
        {
            if (collidableList != null)
            {
                foreach (IShape collidable in collidableList)
                {
                    qTree.Insert(collidable);
                }
            }

        }

        private void QuadTreeUpdate()
        {
            qTree.Clear();
            AddToQuadTree();
        }

        private List<IShape> BroadPhase(IShape shape)
        {
            QuadTreeUpdate();

            qTree.FindPossibleCollisions(PotentialCollisions, shape);

            return PotentialCollisions;
        }

        private void MidPhase(List<IShape> midList, IShape shape)
        {
            iEntity Collider = (iEntity)shape;

            foreach (IShape col in midList)
            {
                if (col.GetBoundingBox().Intersects(shape.GetBoundingBox()) && shape != col)
                {
                    NarrowPhase(col, shape);
                }
                else
                {
                    #region Post Production Code
                    if (shape != col)
                    {
                        // Trigger Exit
                        TriggerExit(Collider, col);
                    }
                    #endregion

                }
            }
        }

        private void NarrowPhase(IShape col1, IShape col2)
        {
            //Get list of each shapes vertices
            List<Vector2> shape1Vertices = col1.GetVertices();
            List<Vector2> shape2Vertices = col2.GetVertices();

            //Add the shape position to each vertex
            List<Vector2> shape1VertPosition = new List<Vector2>();
            foreach (var vert in shape1Vertices)
            {
                shape1VertPosition.Add(col1.GetPosition() + vert);
            }

            List<Vector2> shape2VertPosition = new List<Vector2>();
            foreach (var vert in shape2Vertices)
            {
                shape2VertPosition.Add(col2.GetPosition() + vert);
            }

            //Check for overlap
            bool shape1Overlap = Overlapping(shape1VertPosition, shape2VertPosition);
            bool shape2Overlap = Overlapping(shape2VertPosition, shape1VertPosition);

            #region Post Production Code
            Vector2 MTV = Overlapping2(shape2VertPosition, shape1VertPosition);
            #endregion

            iEntity EntityColider = (iEntity)col2;

            //Test code to add transparency if theres overlap
            if (!shape1Overlap || !shape2Overlap)
            {
                #region Post Production Code
                //Not Colliding
                // Trigger Exit
                if (col1 != col2)
                {
                    TriggerExit(EntityColider, col1);
                }

            }
            else
            {
                //colliding
                EntityColider.isColliding = true;
                EntityColider.CollidingEntity = (iEntity)col1;
                OnCollision?.Invoke(MTV, EntityColider, (iEntity)col1);
                #endregion
            }
        }

        private void CollisionPhases()
        {
            PotentialCollisions.Clear();

            foreach (IShape collisionListener in collisionListeners.ToArray())
            {
                //List of shapes returned from the broad phase
                List<IShape> broadPhase = BroadPhase(collisionListener);

                //Mid phase basic AABB
                MidPhase(broadPhase, collisionListener);
            }
        }

        public void Update()
        {
            foreach (IShape ent in collidableList)
            {
                if (!PotentialCollisions.Contains((iEntity)ent))
                {
                    iEntity tmp = (iEntity)ent;
                    tmp.isColliding = false;
                }
            }

            CollisionPhases();
        }


        //***************************************** New Code written for Post Production *****************************************//

        public void RemoveCollidable(IShape collidable)
        {
            if (collidable.IsCollisionListener())
            {
                if (collisionListeners.Contains(collidable))
                {
                    collisionListeners.Remove(collidable);
                    //return;
                }

            }

            if (collidableList.Contains(collidable))
            {
                collidableList.Remove(collidable);
            }

            if (collisions.Contains(collidable))
            {
                collisions.Remove(collidable);
            }
        }

        public void RemoveAllCollidable()
        {
            collisionListeners.Clear();
            collidableList.Clear();
            collisions.Clear();
        }

        /// <summary>
        /// Deals with Collision when object collides. Preventing them from intersecting
        /// </summary>
        /// <param name="MTV"></param>
        /// <param name="player"></param>
        /// <param name="gameObject"></param>
        void CollisionManager_OnCollision(Vector2 MTV, iEntity player, iEntity gameObject)
        {
            // If IsTrigger true 
            if (player.IsTrigger || gameObject.IsTrigger)
            {
                // OnTriggerEnter
                // If its not in list
                if (!collisions.Contains((IShape)gameObject))
                {
                    // Add to list
                    collisions.Add((IShape)gameObject);
                    collisions.Add(player);
                    // if true call objects OnTriggerEnter
                    if (player.IsTrigger)
                    {
                        player.OnTriggerEnter(gameObject);
                    }

                    if (gameObject.IsTrigger)
                    {
                        gameObject.OnTriggerEnter(player);
                    }
                }
                // if it is in the list means its still colliding and call the on stay
                else if (collisions.Contains((IShape)gameObject))
                {
                    if (player.IsTrigger)
                    {
                        player.OnTriggerStay(gameObject);
                    }

                    if (gameObject.IsTrigger)
                    {
                        gameObject.OnTriggerStay(player);
                    }

                }
                // exit
                return;
            }

            // if IsTriggerWithAutoCollision true
            if (player.IsTriggerWithAutoCollision || gameObject.IsTriggerWithAutoCollision)
            {
                // if its not in collision list
                if (!collisions.Contains((IShape)gameObject))
                {
                    // add to list
                    collisions.Add((IShape)gameObject);
                    collisions.Add(player);
                    // call OnTriggerEnter
                    if (player.IsTriggerWithAutoCollision)
                    {
                        player.OnTriggerEnter(gameObject);
                    }
                    else if (gameObject.IsTriggerWithAutoCollision)
                    {
                        gameObject.OnTriggerEnter(player);
                    }
                }
            }
            // Collision Response
            player.Position += MTV;
        }

        /// <summary>
        /// When object is exiting colliding object 
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="collided"></param>
        void TriggerExit(iEntity collider, IShape collided)
        {
            // Return so it only does trigger and not autocollision
            bool mReturn = false;

            // if contains collider and collided are in the list
            if (collisions.Contains(collided) && collisions.Contains(collider))
            {
                // remove from list
                collisions.Remove(collided);
                collisions.Remove(collider);
                // set to false
                collider.isColliding = false;
                collider.CollidingEntity = null;

                // if collider is trigger true
                if (collider.IsTrigger)
                {
                    // call OnTriggerExit method
                    collider.OnTriggerExit((iEntity)collided);
                    // set mReturn True
                    mReturn = true;
                }

                if (collided.IsTrigger)
                {
                    collided.OnTriggerExit(collider);
                    mReturn = true;
                }

                // if true exit method
                if (mReturn)
                {
                    return;
                }

                // If IsTriggerWithAutoCollision true and not trigger
                if (collider.IsTriggerWithAutoCollision)
                {
                    collider.OnTriggerExit((iEntity)collided);
                }
                else if (collided.IsTriggerWithAutoCollision)
                {
                    collided.OnTriggerExit(collider);
                }
            }
        }
    }
}
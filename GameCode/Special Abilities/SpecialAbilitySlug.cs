using Engine.Collision;
using Engine.Entity;
using Game1.Engine.Pathfinding;
using GameCode.Entities;
using GameCode.Misc;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace GameCode.Special_Abilities
{
    class SpecialAbilitySlug : GameEntity, ISpecialAbility, iCollidable
    {
        #region Data Members

        IPathFinding pathFinding;

        Player mTarget;

        Vector2 mStartPos;
        Vector2 mTargetPos;

        IList<Vector2> path;

        int pathIndex = 0;

        float deltaTime;
        float Acceleration = 300f;
        float timer;

        bool startTimer;
        bool FindPathAgain = true;
        bool reached = false;

        #endregion

        #region Property 

        public bool Active { get; private set; } = true;

        #endregion

        public SpecialAbilitySlug()
        {
            // set texture
            TextureString = @"Abilities\SlugAbility";

            // grab verticies
            Vertices = ReadFile.GetVerticiesFromTSX(TextureString);
            DrawPriority = 5;
        }

        /// <summary>
        /// Begin path finding
        /// </summary>
        /// <param name="pGrid"></param>
        /// <param name="playerPos">players position to start</param>
        /// <param name="targetPos">target positions</param>
        public void Run(IGrid pGrid, Vector2 playerPos, Player targetPos)
        {
            if (pathFinding == null)
            {
                pathFinding = new PathFinding(pGrid);
            }

            mStartPos = playerPos;
            mTarget = targetPos;
            FindPath();
        }

        /// <summary>
        /// Find path to player
        /// </summary>
        void FindPath()
        {
            if (mStartPos == Position)
            {
                path = pathFinding.FindPathWorld(mStartPos, mTarget.Position);
            }
            else
            {
                if (mTarget != null)
                {
                    path = pathFinding.FindPathWorld(Position, mTarget.Position);
                }
            }
            startTimer = true;
        }

        /// <summary>
        /// Move slug towards target
        /// </summary>
        /// <param name="path"></param>
        void Move(IList<Vector2> path)
        {
            if (path.Count > 1)
            {
                if (!reached)
                {
                    Vector2 Direction;
                    try
                    {
                        Direction = path[pathIndex] - Position;
                    }
                    catch
                    {
                        Direction = path[path.Count - 1] - Position;
                    }

                    float Distance = Direction.Length();

                    Direction.Normalize();

                    if (Distance > Direction.Length())
                    {
                        Velocity = Direction * Acceleration;

                        Position += Velocity * deltaTime;
                    }
                    else
                    {
                        if (pathIndex >= path.Count - 1)
                        {
                            Position += Direction;
                            //reached = true;
                            //// if (thread.IsAlive)
                            //// {
                            ////     thread.Abort();
                            ////}
                            //path = null;
                            //Destroy(this);
                            //mTarget = null;
                            //mTargetPos = Vector2.Zero;
                            //Active = false;
                        }
                        else
                        {
                            pathIndex++;
                        }
                    }
                }
            }
            else
            {
                FindPath();

                if (path.Count <= 1)
                {
                    reached = true;
                    path = null;
                    mTarget.Stunned = true;
                    mTarget = null;
                    mTargetPos = Vector2.Zero;
                    Active = false;
                    Destroy(this);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (pathFinding != null)
            {
                if (!reached)
                {
                    if (path != null)
                    {
                        Move(path);
                    }
                }
            }

            if (mTarget != null)
            {
                if (mTarget.Position != mTargetPos)
                {
                    if (FindPathAgain)
                    {
                        Coroutines.Start(Wait());
                    }
                }
            }

            if (startTimer)
            {
                timer += deltaTime;

                if (timer >= 30)
                {
                    //reached = true;
                    path = null;
                    mTarget = null;
                    mTargetPos = Vector2.Zero;
                    Active = false;
                    timer = 0;
                    Destroy(this);
                }
            }

            base.Update(gameTime);
        }

        IEnumerator Wait()
        {
            FindPathAgain = false;
            yield return Coroutines.WaitForSeconds(1f);
            FindPath();
            FindPathAgain = true;
        }
    }
}

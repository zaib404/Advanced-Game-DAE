﻿using DemoCode.Entities;
using Engine;
using Engine.Camera;
using Engine.Collision;
using Engine.Entity;
using Engine.Input;
﻿using Game1;
using Game1.Engine.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DemoCode.Entities
{
    internal class Player : GameEntity, IKeyboardInputObserver, iControllerObserver, iCollidable, ICameraSubject
    {

        #region Data Members

        private List<BasicInput> inputOptions = new List<BasicInput>
        {
            new BasicInput(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.E),
            new BasicInput(Keys.I, Keys.K, Keys.J, Keys.L, Keys.None, Keys.O),
            new BasicInput(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.None, Keys.End),
            new BasicInput(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.None, Keys.NumPad9)
        };

        private BasicInput inputKeys;
        private static int playerCount = 0;

        private float acceleration = 2f;

        private uint abilityTimer = 0;
        private bool abilityTimeout = false;

        private uint walkTimer = 0;
        private bool walkTimeout = false;

        private bool sprintActive = false;

        GamePadInput inputButtons = new GamePadInput(Buttons.RightThumbstickRight, Buttons.RightThumbstickLeft, Buttons.X, Buttons.RightTrigger);

        #endregion

        
        IPathFinding pathFinder;

        iEntity Star;

        public Player()
        {
            if (GamePad.GetCapabilities(playerCount).IsConnected)
            {
                ControllerInput.Subscribe(this, inputButtons.allButtons, playerCount);
            }
            else
            {
                BasicInput keys;

                try
                {
                    keys = inputOptions[playerCount];
                }
                catch
                {
                    keys = new BasicInput();
                }

                KeyboardInput.Subscribe(this, keys.allKeys);
                inputKeys = keys;
                acceleration = 8f;
            }

            playerCount++;
            
            //CollisionManager.subCollision(this);

            DrawPriority = 1f;

            listenToCollisions = true;
        }

        public void input(Keys key)
        {
            if (inputKeys.allKeys.Contains(key))
            {
                if (key == inputKeys.up)
                {
                    Velocity = new Vector2(0, -1 * acceleration);
                }
                else if (key == inputKeys.down)
                {
                    Velocity = new Vector2(0, acceleration);
                }
                else if (key == inputKeys.left)
                {
                    Velocity = new Vector2(-1 * acceleration, 0);
                }
                else if (key == inputKeys.right)
                {
                    Velocity = new Vector2(acceleration, 0);
                }
                else if (key == inputKeys.use)
                {
                    if (!abilityTimeout)
                    {
                        var pos = pathFinder.FindPathWorld(Position, Star.Position);

                        FollowPath(pos);
                    }

                }
                else if(key == inputKeys.sprint)
                {
                    if (!walkTimeout)
                    {
                        walkTimeout = true;

                        if (acceleration == 0.5f)
                        {
                            acceleration = 8f;
                        }
                        else if(acceleration == 8f)
                        {
                            acceleration = 0.5f;
                        }
                    }
                }

                Position += Velocity;
                
            }

            Velocity = new Vector2(0, 0);
        }

        void FollowPath(IList<Vector2> pos)
        {
            bool reach = false;
            int WayPointIndex = 0;

            if (pos.Count > 0)
            {
                if (!reach)
                {
                    float Distance = Vector2.Distance(Position, pos[WayPointIndex]);
                    Vector2 Direction = pos[WayPointIndex] - Position;
                    Direction.Normalize();

                    OnEntityRequested(Position, "Walls/BrownPath", typeof(BrownPath));

                    if (Distance > Direction.Length())
                        Position += Direction * (acceleration);
                    else
                    {
                        if (WayPointIndex >= pos.Count - 1)
                        {
                            Position += Direction;
                            reach = true;
                        }
                        else
                            WayPointIndex++;
                    }

                }
            }

        }


        public override void Update(GameTime gameTime)
        {
            #region remove this dirty ass code after prototyping

            Vector2 vel = new Vector2(0, 0);

            if (isColliding)
            {
                if (Position.X > CollidingEntity.Position.X)
                {
                    vel = new Vector2(4, 0);
                }
                else
                {
                    vel = new Vector2(-4, 0);
                }

                if (Position.Y > CollidingEntity.Position.Y)
                {
                    vel = new Vector2(0, 4);
                }
                else
                {
                    vel = new Vector2(0, -4);
                }
            }
            isColliding = false;

            Position += vel;

            #endregion


            if (abilityTimeout)
            {
                abilityTimer++;
            }

            if (abilityTimer >= 300)
            {
                abilityTimeout = false;
                abilityTimer = 0;
            }

            if (walkTimeout)
            {
                walkTimer++;
            }

            if (walkTimer >= 100)
            {
                walkTimeout = false;
                walkTimer = 0;
            }

        }

        private void Rotate(Vector2 val)
        {
            Rotation = val.X;
            // this rotation was testing
            if (val.X > 0)
            {
                Rotation += val.X;
            }
            else
            {
                Rotation -= val.X;
            }
        }

        public void gamePadInput(Buttons gamePadButtons, GamePadThumbSticks thumbSticks)
        {
            var leftThumbX = thumbSticks.Left.X * acceleration;
            var leftThumbY = thumbSticks.Left.Y * -1 * acceleration;

            Vector2 vel = new Vector2(leftThumbX, leftThumbY);

            if (sprintActive)
            {
                vel = vel * 3;
            }


            if (gamePadButtons == inputButtons.rotateCW)
            {
                //Rotate(thumbSticks.Right);
            }
            else if (gamePadButtons == inputButtons.rotateACW)
            {
                //Rotate(thumbSticks.Right);
            }
            else if (gamePadButtons == inputButtons.sprint)
            {
                sprintActive = true;
            }
            else if (gamePadButtons == inputButtons.use)
            {
                if (!abilityTimeout)
                {
                    OnEntityRequested(new Vector2(Position.X + 80, Position.Y), "Walls/wall-left", typeof(Wall));
                    abilityTimeout = true;
                }
            }
            else
            {
                sprintActive = false;
            }

            Position += vel;
        }


        public void injectPathFinding(IPathFinding pPathFinder, iEntity pStar)
        {
            pathFinder = pPathFinder;
            Star = pStar;
        }
    }
}

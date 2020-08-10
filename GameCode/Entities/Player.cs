using Engine.Camera;
using Engine.Collision;
using Engine.Engine.Raycasting;
using Engine.Entity;
using Engine.Input;
using Game1.Engine.Pathfinding;
using GameCode.Misc;
using GameCode.Special_Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace GameCode.Entities
{
    public class Player : GameEntity, IKeyboardInputObserver, iControllerObserver, iCollidable, ICameraSubject
    {
        #region Data Members

        #region Input keys
        GamePadInput inputButtons = new GamePadInput(Buttons.RightThumbstickRight, Buttons.RightThumbstickLeft, Buttons.X, Buttons.RightTrigger);

        private List<BasicInput> inputOptions = new List<BasicInput>
        {
            // player 1
            new BasicInput(
                Keys.W, // Up
                Keys.S, // Down
                Keys.A, // Left
                Keys.D, //Right
                Keys.None, // Sprint
                Keys.LeftShift, // Attack
                Keys.E), // Use
            // player 2
            new BasicInput(
                Keys.Up,
                Keys.Down,
                Keys.Left,
                Keys.Right,
                Keys.None,
                Keys.RightShift,
                Keys.RightControl),
            // player 3
            new BasicInput(Keys.I,
                Keys.K,
                Keys.J,
                Keys.L,
                Keys.None,
                Keys.Space,
                Keys.O),
            // player 4
            new BasicInput(Keys.NumPad8,
                Keys.NumPad5,
                Keys.NumPad4,
                Keys.NumPad6,
                Keys.None,
                Keys.NumPad7,
                Keys.NumPad9)
        };

        private BasicInput inputKeys;
        #endregion

        PathFinding pathFinding;
        ISpecialAbilityManager abilityManager;

        string playerPathTexture;

        int vertical, horizontal, lastDirV, lastDirH;

        float deltaTime;
        float stunTimer = 3;
        float dashTimer = 5;
        float abilityTimer = 5;

        bool dashActive = false;
        bool collisionDash = false;
        bool abilityActive = false;
        bool StartCorotine = false;
        bool canUseAbility = false;

        #endregion

        #region Properties

        public int playerCount { get; private set; }

        public float Acceleration { get; set; } = 400f;

        public bool HasArtefact { get; set; } = false;

        public bool Stunned { get; set; } = false;

        public Player ArtefactHolder { get; set; }

        #endregion

        public Player()
        {
            //SetUpKeys();

            //playerCount++;
            DrawPriority = 5f;

            listenToCollisions = true;
            IsTriggerWithAutoCollision = true;
            //PathTexture();
            Coroutines.Start(AbilityStart());
        }

        /// <summary>
        /// Set player count and set up keys and its trail path texture
        /// </summary>
        /// <param name="pCount"></param>
        public void SetPlayerCount(int pCount)
        {
            playerCount = pCount;
            SetUpKeys();
            PathTexture();
        }

        /// <summary>
        /// Set controller or keyboard
        /// </summary>
        void SetUpKeys()
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
            }
        }

        /// <summary>
        /// set path
        /// </summary>
        void PathTexture()
        {
            if (playerCount == 0)
            {
                playerPathTexture = "PlayersPath/Player1Path";
            }
            else if (playerCount == 1)
            {
                playerPathTexture = "PlayersPath/Player2Path";
            }
            else if (playerCount == 2)
            {
                playerPathTexture = "PlayersPath/Player3Path";
            }
            else if (playerCount == 3)
            {
                playerPathTexture = "PlayersPath/Player4Path";
            }
        }

        /// <summary>
        /// Input deals with movement, attacking and calling abilities
        /// </summary>
        /// <param name="key"></param>
        public void input(Keys key)
        {
            // if not stunned
            if (!Stunned)
            {
                if (inputKeys.allKeys.Contains(key))
                {
                    //Get direction player travelling
                    // up = -1, down = 1, right = 1, left = -1
                    vertical = key == inputKeys.up ? -1 : key == inputKeys.down ? 1 : 0;
                    horizontal = key == inputKeys.left ? -1 : key == inputKeys.right ? 1 : 0;
                    // get last direction
                    if (horizontal != 0 || vertical != 0)
                    {
                        lastDirH = horizontal;
                        lastDirV = vertical;
                    }

                    if (key == inputKeys.up || key == inputKeys.down || key == inputKeys.left || key == inputKeys.right)
                    {
                        Move();
                    }
                    else if (key == inputKeys.attack)
                    {
                        if (!dashActive)
                        {
                            dashActive = true;
                            // start coroutine
                            Coroutines.Start(Dash(lastDirH, lastDirV));
                        }
                    }
                    else if (key == inputKeys.use)
                    {
                        if (canUseAbility)
                        {
                            if (!abilityActive)
                            {
                                abilityActive = true;
                                SpecialAbility(lastDirH, lastDirV);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deals with player movement
        /// </summary>
        void Move()
        {
            // set velocity to 1 or 0
            Velocity = new Vector2(horizontal, vertical);
            // times acceleration
            Velocity *= Acceleration;
            // summon path
            OnEntityRequested(Position, playerPathTexture, typeof(PlayerPath));

            Position += Velocity * deltaTime;
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Calls forth players ability
        /// </summary>
        /// <param name="x">last X direction</param>
        /// <param name="y">last y direction</param>
        void SpecialAbility(int x, int y)
        {
            //place wall, ice block, cocaine slug, adrenaline shot
            abilityManager.CallSpecialAbilty(this, x, y);
            Coroutines.Start(AbilityWait());
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Coroutines.Running)
            {
                Coroutines.Update();
            }

            if (Stunned)
            {
                stunTimer -= deltaTime;
                // 3 second stun
                if (stunTimer <= 0)
                {
                    Stunned = false;
                    stunTimer = 3;
                    Transparency = 1;
                    StartCorotine = false;
                }
                else
                {
                    if (!StartCorotine)
                    {
                        Coroutines.Start(Stun());
                        StartCorotine = true;
                    }
                }
            }
        }

        public override void OnTriggerEnter(iEntity gameObject)
        {
            // if playyer
            if (gameObject is Player p)
            {
                // if active and collision dash not active then
                if (dashActive && !collisionDash)
                {
                    collisionDash = true;
                    // stun other player
                    p.Stunned = true;
                    // if p has artefact
                    if (p.HasArtefact)
                    {
                        // take it off them and become the one who has the artefact
                        p.HasArtefact = false;
                        HasArtefact = true;
                        ArtefactHolder = this;
                        p.ArtefactHolder = this;
                    }

                    Coroutines.Start(p.Stun());
                }
                // if enemy dashes
                else if (p.dashActive && !p.collisionDash)
                {
                    p.collisionDash = true;

                    Stunned = true;

                    if (HasArtefact)
                    {
                        HasArtefact = false;
                        p.HasArtefact = true;
                        p.ArtefactHolder = p;
                        ArtefactHolder = p;
                    }

                    Coroutines.Start(Stun());
                }
            }
        }

        #region IEnumerator

        /// <summary>
        /// Do the dash
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IEnumerator Dash(int x, int y)
        {
            Vector2 newVel;
            float dashDistance = 200f;

            newVel = new Vector2(x, y);
            newVel *= dashDistance;

            Vector2 newPos = newVel;

            // Do raycast to make sure its not colliding with anything
            var raycast = RayCast2D.RayCast(Position, new Vector2(x, y), dashDistance, pathFinding.mGrid);

            // if collide with something then set position by there
            if (raycast.DoCollide)
            {
                Position = raycast.Position;
            }
            else
            {
                Position += newPos;
            }

            yield return Coroutines.WaitForSeconds(dashTimer);

            dashActive = false;
            collisionDash = false;
        }

        /// <summary>
        /// Ability cool down
        /// </summary>
        /// <returns></returns>
        IEnumerator AbilityWait()
        {
            yield return Coroutines.WaitForSeconds(abilityTimer);
            abilityActive = false;
            // destroy ability
            abilityManager.CallDestroy(this, 0);
        }

        /// <summary>
        /// Show player stunned by making player
        /// flash
        /// </summary>
        /// <returns></returns>
        IEnumerator Stun()
        {
            while (Stunned)
            {
                Transparency /= 2;
                yield return Coroutines.WaitForSeconds(.3f);
                Transparency *= 2;
                yield return Coroutines.WaitForSeconds(.2f);
            }
        }

        /// <summary>
        /// When starting level prevent players from using abilities straight away
        /// </summary>
        /// <returns></returns>
        IEnumerator AbilityStart()
        {
            yield return Coroutines.WaitForSeconds(10);
            canUseAbility = true;
        }

        #endregion

        #region Injections

        public void InjectGrid(IGrid iGrid)
        {
            pathFinding = new PathFinding(iGrid);
        }

        public void InjectAbility(ISpecialAbilityManager pAbilityManager)
        {
            abilityManager = pAbilityManager;
            abilityManager.GiveAbilty(this, pathFinding.mGrid);
        }

        #endregion

        #region Game Controller
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
            var leftThumbX = thumbSticks.Left.X * Acceleration;
            var leftThumbY = thumbSticks.Left.Y * -1 * Acceleration;

            Vector2 vel = new Vector2(leftThumbX, leftThumbY);

            //if (sprintActive)
            //{
            //    vel = vel * 3;
            //}


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
                //sprintActive = true;
            }
            else if (gamePadButtons == inputButtons.use)
            {
                //if (!abilityTimeout)
                //{
                //    OnEntityRequested(new Vector2(Position.X + 80, Position.Y), "Walls/wall-left", typeof(Wall));
                //    abilityTimeout = true;
                //}
            }
            else
            {
                //sprintActive = false;
            }

            Position += vel;
        }
        #endregion

    }
}

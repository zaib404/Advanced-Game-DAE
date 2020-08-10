using Engine.Shape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Engine.Entity
{

    /// <summary>
    /// Abstraction for any game entity
    /// </summary>
    public abstract class Entity : iEntity, IShape
    {
        #region EventHandlers
        public event EventHandler<EntityRequestArgs> EntityRequested;

        /// <summary>
        /// Event handler for requesting an entity
        /// </summary>
        /// <param name="pos">Position of the entity to add</param>
        /// <param name="texture">Texture of the entity</param>
        /// <param name="pType">The Type of entity to add</param>
        public virtual iEntity OnEntityRequested(Vector2 pos, string texture, Type pType)
        {
            //***************************************** Slight change for Post Production *****************************************//
            // Instead of void now returns ientity
            EntityRequestArgs requestArgs = new EntityRequestArgs() { Position = pos, Texture = texture, type = pType };
            EntityRequested?.Invoke(this, requestArgs);
            return requestArgs.gameObject;
        }

        public event EventHandler<EventArgs> LevelFinished;

        public virtual void OnLevelFinished()
        {
            LevelFinished?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Properties

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Velocity
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;

        } = 0f;

        public Vector2 Origin
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        public string TextureString
        {
            get;
            set;
        }

        public Guid UID
        {
            get;
            set;
        }

        public String UName
        {
            get;
            set;
        }

        public float maxHealth
        {
            get;
            set;
        }

        public float currentHealth
        {
            get;
            set;
        }

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X, (int)Position.Y,
                    Texture.Width, Texture.Height);
            }
        }

        public bool Visible
        {
            get;
            set;
        }

        public float DrawPriority
        {
            get;
            set;
        } = 0f;

        public float Transparency
        {
            get;
            set;
        } = 1f;

        public bool isColliding
        {
            get;
            set;
        }

        public iEntity CollidingEntity
        {
            get;
            set;
        }
        #endregion

        #region Input structs

        /// <summary>
        /// Basic keyboard input struct
        /// </summary>
        public struct BasicInput
        {
            public BasicInput(Keys pUp = Keys.None,
                Keys pDown = Keys.None,
                Keys pLeft = Keys.None,
                Keys pRight = Keys.None,
                Keys pSprint = Keys.None,
                Keys pAttack = Keys.None,
                Keys pUse = Keys.None,
                Keys pRotate = Keys.None)
            {
                up = pUp;
                down = pDown;
                left = pLeft;
                right = pRight;
                sprint = pSprint;
                attack = pAttack;
                use = pUse;
                rotate = pRotate;

                allKeys = new List<Keys>(new Keys[] { up, down, left, right, sprint, attack, use, rotate });
                allKeys.RemoveAll((Keys key) => key == Keys.None);
            }

            public Keys up;
            public Keys down;
            public Keys left;
            public Keys right;
            public Keys sprint;
            public Keys attack;
            public Keys use;
            public Keys rotate;

            public List<Keys> allKeys;

        }

        /// <summary>
        /// Basic GamePadInput struct for managing assigned gamepad keys
        /// </summary>
        public struct GamePadInput
        {
            public Buttons rotateCW;
            public Buttons rotateACW;
            public Buttons sprint;
            public Buttons use;

            public List<Buttons> allButtons;

            public GamePadInput(Buttons pRotateCW = 0, Buttons pRotateACW = 0, Buttons pUse = 0, Buttons pSprint = 0)
            {
                rotateCW = pRotateCW;
                rotateACW = pRotateACW;
                use = pUse;
                sprint = pSprint;

                allButtons = new List<Buttons>(new Buttons[] { rotateCW, rotateACW, use, sprint, });
                allButtons.RemoveAll((Buttons key) => key == 0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public Entity()
        {
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Entity()
        {
            Console.WriteLine("HAHAHA - I have destoryed the entity");
        }

        /// <summary>
        /// Used to set the entities 
        /// UID and UName
        /// </summary>
        /// <param name="id">Unique Identifier</param>
        /// <param name="name">Unique Name</param>
        public void Setup(Guid id, string name)
        {
            UID = id;
            UName = name;
        }

        /// <summary>
        /// Setup the entity and set properties
        /// </summary>
        /// <param name="id">The UID of the entity</param>
        /// <param name="name">The UName of the entity</param>
        /// <param name="tex">The texture of the entity</param>
        /// <param name="pos">The starting postion of the entity</param>
        public void Setup(Guid id, string name, string tex, Vector2 pos, List<Vector2> verts = default(List<Vector2>))
        {
            UID = id;
            UName = name;
            TextureString = tex;
            Position = pos;

            if (verts != default(List<Vector2>))
            {
                Vertices = verts;
            }
            else
            {
                if (Vertices == null)
                {
                    Vertices = new List<Vector2>() { new Vector2(0, 0), new Vector2(50, 0), new Vector2(50, 50), new Vector2(0, 50) };
                }


                //Vertices = new List<Vector2>();
            }

        }

        /// <summary>
        /// This will distribute it to all other classes 
        /// In the hierarchy and any class which implements 
        /// An update with the same signature.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region IShape

        public List<Vector2> Vertices
        {
            get;
            set;
        }

        public List<Vector2> GetVertices()
        {
            return Vertices;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public Rectangle GetBoundingBox()
        {
            return HitBox;
        }

        protected bool listenToCollisions = false;

        public bool IsCollisionListener()
        {
            return listenToCollisions;
        }

        #endregion


        //***************************************** New Code written for Post Production *****************************************//

        /// <summary>
        /// If set True it will not run collision
        /// and it will call OnTriggerEnter, OnTriggerStay and OnTriggerExit
        /// </summary>
        public bool IsTrigger
        {
            get;
            protected set;
        } = false;

        /// <summary>
        /// If set true only OnTriggerEnter and OnTriggerExit will be called
        /// and Collision will be handled by CollisionManager.
        /// Note: If Objects IsTrigger - true only IsTrigger will be called
        /// </summary>
        public bool IsTriggerWithAutoCollision
        {
            get;
            protected set;
        } = false;

        /// <summary>
        /// 'IsTrigger' must be set to true, for this to run
        /// On colliding with object the enter stage
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void OnTriggerEnter(iEntity gameObject) { }

        /// <summary>
        /// 'IsTrigger' must be set to true, for this to run
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void OnTriggerStay(iEntity gameObject) { }

        /// <summary>
        /// 'IsTrigger' must be set to true, for this to run
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void OnTriggerExit(iEntity gameObject) { }

        public event EventHandler<EventArgs> EntityDestroy;

        public virtual void Destroy(iEntity ent)
        {
            EntityDestroy?.Invoke(ent, new EventArgs());
        }

        public void Dispose()
        {

        }
    }
}
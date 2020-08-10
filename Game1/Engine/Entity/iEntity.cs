using Engine.Shape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Entity
{
    /// <summary>
    /// Contract for any game entity
    /// </summary>
    public interface iEntity : IShape, IDisposable
    {
        Guid UID { get; set; }
        String UName { get; set; }

        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        Texture2D Texture { get; set; }
        string TextureString { get; set; }

        bool Visible { get; set; }
        bool isColliding { get; set; }
        iEntity CollidingEntity { get; set; }
        Rectangle HitBox { get; }
        float DrawPriority { get; set; }
        float Transparency { get; set; }

        event EventHandler<EntityRequestArgs> EntityRequested;
        event EventHandler<EventArgs> LevelFinished;

        void Setup(Guid id, string name);
        void Setup(Guid id, string name, string texture, Vector2 position, List<Vector2> verts = default(List<Vector2>));
        void Update(GameTime gameTime);
        #region Post Production Code
        event EventHandler<EventArgs> EntityDestroy;
        void Destroy(iEntity entity);
        iEntity OnEntityRequested(Vector2 pos, string texture, Type pType);
        #endregion

    }
}

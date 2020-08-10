using Microsoft.Xna.Framework;
using System;

namespace Engine.Entity
{
    /// <summary>
    /// Class for custom EntityRequestArgs
    /// </summary>
    public class EntityRequestArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public string Texture { get; set; }
        public Type type { get; set; }
        #region Post Production Code
        public iEntity gameObject { get; set; }
        #endregion
    }
}
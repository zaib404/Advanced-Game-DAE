using Engine.Collision;
using Engine.Entity;
using Microsoft.Xna.Framework;

namespace GameCode.Entities
{
    class ClosedDoor : GameEntity, iCollidable
    {
        #region Data Members

        float deltaTime;
        float timer;

        #endregion

        public ClosedDoor()
        {
            DrawPriority = 2;
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += deltaTime;

            // open doors after 5 seconds
            if (timer >= 5)
            {
                OnEntityRequested(Position, "Door/OpenDoor", typeof(OpenDoor));
                Destroy(this);
            }
        }

    }
}

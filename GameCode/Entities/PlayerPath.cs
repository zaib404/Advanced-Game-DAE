using Engine.Entity;
using Microsoft.Xna.Framework;

namespace GameCode.Entities
{
    class PlayerPath : GameEntity
    {
        #region Data Members

        float deltaTime;

        float timer;
        float TimerBeforeTransparencyBegins = 3;
        float wait = 1;

        bool beginTransparency = false;

        #endregion

        public PlayerPath() { DrawPriority = 3; }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += deltaTime;

            if (timer >= TimerBeforeTransparencyBegins)
            {
                beginTransparency = true;
            }

            if (beginTransparency)
            {
                if (timer >= wait)
                {
                    // fade away
                    Transparency -= deltaTime;

                    if (Transparency < 0)
                    {
                        Destroy(this);
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}

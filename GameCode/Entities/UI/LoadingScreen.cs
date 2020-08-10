using Engine.Entity;
using Engine.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameCode.Entities
{
    /// <summary>
    /// Fake loading screen
    /// </summary>
    class LoadingScreen : GameEntity, IStaticUI
    {
        #region Data Members

        private float deltaTime;
        private float timer;
        private int i;

        List<LoadingBar> bar = new List<LoadingBar>();
        // starting position of bar
        Vector2 barPos = new Vector2(195, 550);

        #endregion

        #region Properties

        public bool load { get; set; }

        #endregion

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += deltaTime;

            if (timer > .5f)
            {
                if (i <= 9)
                {
                    Loading();
                    i++;
                }
                else
                {
                    OnLevelFinished();
                }
                timer = 0;
            }
        }

        void Loading()
        {
            bar.Add(OnEntityRequested(barPos, "Loading/LoadingBar", typeof(LoadingBar)) as LoadingBar);
            barPos.X += bar.Last().HitBox.Width;
            bar.Last().Position = barPos;
        }
    }
}

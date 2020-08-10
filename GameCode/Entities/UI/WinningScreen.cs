using Engine.Entity;
using Engine.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameCode.Entities
{
    /// <summary>
    /// Winning screen showing who won
    /// </summary>
    class WinningScreen : GameEntity, IStaticUI
    {
        #region Data Members

        private float deltaTime;
        private float timer;
        private int i;

        List<LoadingBar> bar = new List<LoadingBar>();
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

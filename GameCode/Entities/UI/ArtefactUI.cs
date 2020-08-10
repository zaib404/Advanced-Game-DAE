using Engine.Entity;
using Engine.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameCode.Entities
{
    class ArtefactUI : GameEntity, IStaticUI
    {
        #region Data Members

        // list of all players
        List<Player> AllPlayers = new List<Player>();

        // list of positions ui can move to
        List<Vector2> mArtefactPosition;

        // sort pos once
        bool sortPos = false;

        #endregion

        #region Properties

        public Player mPlayer { get; set; }
        public bool mNewPlayerHolding { get; set; } = false;

        #endregion

        public ArtefactUI()
        {
            Transparency = 0;
        }

        public void InjectPlayers(Player pPlayer)
        {
            AllPlayers.Add(pPlayer);
        }

        /// <summary>
        /// Set artefact positions
        /// </summary>
        void ArtefactPositions()
        {
            if (AllPlayers.Count <= 1)
            {
                mArtefactPosition = new List<Vector2>() { new Vector2(0, 0) };
            }
            else if (AllPlayers.Count == 2)
            {
                mArtefactPosition = new List<Vector2>() { new Vector2(0, 0), new Vector2(GameMain.ScreenWidth - HitBox.Width, 450) };
            }
            else if (AllPlayers.Count == 3)
            {
                mArtefactPosition = new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(GameMain.ScreenWidth - HitBox.Width,0),
                    new Vector2(400, 450)
                };
            }
            else
            {
                mArtefactPosition = new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(GameMain.ScreenWidth - HitBox.Width,0),
                    new Vector2(0, 450),
                    new Vector2(GameMain.ScreenWidth - HitBox.Width, 450)
                };
            }
        }

        /// <summary>
        /// Set position of ui depending on who has it
        /// </summary>
        void SetPosition()
        {
            int playerNum = AllPlayers.FindIndex(x => x == mPlayer);

            Position = mArtefactPosition[playerNum];

            Transparency = 5;

            mNewPlayerHolding = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!sortPos)
            {
                // sort position out once
                ArtefactPositions();
                sortPos = true;
            }

            // new player holds artefact
            if (mNewPlayerHolding)
            {
                // if not null
                if (mPlayer != null)
                {
                    SetPosition();
                }
            }

            base.Update(gameTime);
        }

    }
}

using Engine.Collision;
using Engine.Entity;
using Microsoft.Xna.Framework;

namespace GameCode.Entities
{
    class Artefact : GameEntity, iCollidable
    {
        #region Data Members

        // ref to artefact ui
        ArtefactUI artefactUI;

        #endregion

        #region Properties 

        // player who holds the artefact
        public Player mPlayer { get; private set; }
        // has it been picked up
        public bool artefactPickedUp { get; private set; } = false;

        #endregion

        public Artefact()
        {
            // no collision response needed
            IsTrigger = true;
            DrawPriority = 2;
        }

        public void InjectUI(ArtefactUI pArt)
        {
            artefactUI = pArt;
        }

        public override void OnTriggerEnter(iEntity gameObject)
        {
            if (gameObject is Player p)
            {
                // if artefact hasnt been picked up then
                if (!artefactPickedUp)
                {
                    // set player who has it
                    mPlayer = p;
                    // set to true
                    mPlayer.HasArtefact = true;
                    artefactPickedUp = true;
                    // hide artefact
                    Transparency = 0;
                    // artefact ui holds ref to player
                    artefactUI.mNewPlayerHolding = true;
                    artefactUI.mPlayer = mPlayer;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // if player isntt null
            if (mPlayer != null)
            {
                // if player isnt the artefact holder then
                if (mPlayer.ArtefactHolder != mPlayer)
                {
                    // set new artefact holder
                    if (mPlayer.ArtefactHolder == null)
                    {
                        mPlayer.ArtefactHolder = mPlayer;
                    }
                    else
                    {
                        mPlayer = mPlayer.ArtefactHolder;
                    }

                    artefactUI.mNewPlayerHolding = true;
                    artefactUI.mPlayer = mPlayer;
                }
            }

            base.Update(gameTime);
        }
    }
}

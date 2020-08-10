using Engine.Collision;
using Engine.Entity;
using Microsoft.Xna.Framework;

namespace GameCode.Entities
{
    class Patient : GameEntity, iCollidable
    {
        #region DataMembers
        Artefact mArtefact;
        bool show = false;
        #endregion

        public Patient()
        {
            // set texture
            TextureString = @"Patient\Patient";

            // get Vertices from tsx file
            //Vertices = ReadFile.GetVerticiesFromTSX(TextureString);

            Transparency = 0f;

            DrawPriority = 3;

            IsTrigger = true;
        }

        public void InjectArtefact(Artefact pArt)
        {
            mArtefact = pArt;
        }

        public override void Update(GameTime gameTime)
        {
            if (!show && mArtefact != null)
            {
                if (mArtefact.artefactPickedUp)
                {
                    Transparency = 1;
                    show = true;
                }
            }

            base.Update(gameTime);
        }

        public override void OnTriggerStay(iEntity gameObject)
        {
            if (mArtefact.artefactPickedUp && mArtefact.mPlayer == gameObject as Player)
            {
                mArtefact.mPlayer.OnLevelFinished();
            }
        }
    }
}

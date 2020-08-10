using Engine.Entity;
using GameCode.Entities;
using GameCode.Misc;
using Microsoft.Xna.Framework;
using System.Collections;

namespace GameCode.Special_Abilities
{
    class SpecialAbilityAdrenalineShot : GameEntity, ISpecialAbility
    {
        #region Data Members

        Player mPlayer;

        float mDosage = 200f;

        float mDosageDecrease;

        int DosageTimeLimit = 5;

        float originalAcceleration;

        #endregion

        #region Properties 

        public bool DosageActive { get; set; } = false;

        #endregion

        public SpecialAbilityAdrenalineShot()
        {
            // set texture
            TextureString = @"Abilities\AdrenalineShotAbility";

            // grab verticies
            Vertices = ReadFile.GetVerticiesFromTSX(TextureString);

            IsTrigger = true;

            DrawPriority = 7;
        }

        /// <summary>
        /// Activate ability and speed up the player
        /// </summary>
        /// <param name="pPlayer"></param>
        public void DoseMeUp(Player pPlayer)
        {
            mPlayer = pPlayer;

            originalAcceleration = mPlayer.Acceleration;

            mPlayer.Acceleration += mDosage;
            DosageActive = true;

            mDosageDecrease = originalAcceleration / DosageTimeLimit;
            mDosageDecrease /= 2;

            Coroutines.Start(EffectsWearingOff());
        }

        /// <summary>
        /// When to start 
        /// </summary>
        /// <returns></returns>
        IEnumerator EffectsWearingOff()
        {
            float timer = 0;

            while (true)
            {
                yield return Coroutines.WaitForSeconds(1);
                timer += 1;

                mPlayer.Acceleration -= mDosageDecrease;

                if (timer >= 5)
                {
                    mPlayer.Acceleration = originalAcceleration;
                    mPlayer = null;
                    DosageActive = false;
                    Destroy(this);
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (mPlayer != null)
            {
                Position = mPlayer.Position;
            }

            if (Coroutines.Running)
            {
                Coroutines.Update();
            }

            base.Update(gameTime);
        }
    }
}

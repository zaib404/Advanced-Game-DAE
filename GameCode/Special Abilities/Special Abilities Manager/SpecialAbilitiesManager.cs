using Game1.Engine.Pathfinding;
using GameCode.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameCode.Special_Abilities
{
    public class SpecialAbilityManager : ISpecialAbilityManager
    {
        #region Data Members

        // keep track of all players
        static Dictionary<ISpecialAbility, Player> mPlayers = new Dictionary<ISpecialAbility, Player>();

        IList<ISpecialAbility> mAbilitiesActive = new List<ISpecialAbility>();

        IPathFinding pathFinding;

        int abilityNum;

        #endregion

        #region Properties

        public ISpecialAbility PlayerAbility { get; private set; }

        #endregion

        public SpecialAbilityManager() { }

        /// <summary>
        /// Assign players their abilities
        /// </summary>
        /// <param name="pPlayer">Ref of the one who holds the ability</param>
        /// <param name="pGrid">Reference to grid</param>
        public void GiveAbilty(Player pPlayer, IGrid pGrid)
        {
            // count
            abilityNum = mPlayers.Count + 1;

            if (abilityNum == 1)
            {
                // Player 1 has been gifted the wall
                PlayerAbility = new SpecialAbilityWall();
                mPlayers.Add(PlayerAbility, pPlayer);
            }
            else if (abilityNum == 2)
            {
                // Player 2 has been gifted the ice
                PlayerAbility = new SpecialAbilityIce();
                mPlayers.Add(PlayerAbility, pPlayer);
            }
            else if (abilityNum == 3)
            {
                // Player 3 has been gifted the Slug
                PlayerAbility = new SpecialAbilitySlug();
                mPlayers.Add(PlayerAbility, pPlayer);
            }
            else if (abilityNum == 4)
            {
                // Player 4 has been gifted the AdrenalineShot
                PlayerAbility = new SpecialAbilityAdrenalineShot();
                mPlayers.Add(PlayerAbility, pPlayer);
            }

            pathFinding = new PathFinding(pGrid);
        }

        /// <summary>
        /// Call Forth the players ability
        /// Determin who to call
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pDirX"></param>
        /// <param name="pDirY"></param>
        public void CallSpecialAbilty(Player pPlayer, int pDirX, int pDirY)
        {
            if (mPlayers.ContainsValue(pPlayer))
            {
                if (abilityNum == 1)
                {
                    SummonWall(pPlayer, pDirX, pDirY);
                }
                else if (abilityNum == 2)
                {
                    SummonIce(pPlayer, pDirX, pDirY);
                }
                else if (abilityNum == 3)
                {
                    SummonSlug(pPlayer);
                }
                else if (abilityNum == 4)
                {
                    SummonAdrenalineShot(pPlayer);
                }
            }
        }

        /// <summary>
        /// Summon AdrenalineShot giving player speed boost
        /// </summary>
        /// <param name="pPlayer"></param>
        void SummonAdrenalineShot(Player pPlayer)
        {
            // if ability count is greater than 0
            if (mAbilitiesActive.Count > 0)
            {
                // if its not active then then remove from list
                if (!((SpecialAbilityAdrenalineShot)mAbilitiesActive.Last()).DosageActive)
                {
                    mAbilitiesActive.Remove(mAbilitiesActive.Last());
                }
            }

            if (mAbilitiesActive.Count < 1)
            {
                // summon AdrenalineShot
                mAbilitiesActive.Add(PlayerAbility.OnEntityRequested(pPlayer.Position, PlayerAbility.TextureString, PlayerAbility.GetType()) as SpecialAbilityAdrenalineShot);
                ((SpecialAbilityAdrenalineShot)mAbilitiesActive.Last()).DoseMeUp(pPlayer);
            }

        }

        /// <summary>
        /// Summon the slug
        /// </summary>
        /// <param name="pPlayer"></param>
        void SummonSlug(Player pPlayer)
        {
            if (mAbilitiesActive.Count > 0)
            {
                if (!((SpecialAbilitySlug)mAbilitiesActive.Last()).Active)
                {
                    mAbilitiesActive.Remove(mAbilitiesActive.Last());
                }
            }

            if (mAbilitiesActive.Count < 1)
            {
                Vector2 pos = new Vector2();

                // set position for slug in first place it can be set
                for (int i = 0; i < 5; i++)
                {
                    var listNode = pathFinding.mGrid.GetNeighbourNodes(pathFinding.mGrid.ConvertWorldPosToGrid(pPlayer.Position));

                    foreach (var item in listNode)
                    {
                        if (item.Walkable)
                        {
                            pos = item.Position;
                            break;
                        }
                    }

                    if (pos != Vector2.Zero)
                    {
                        break;
                    }
                }

                if (pos != Vector2.Zero)
                {
                    mAbilitiesActive.Add(PlayerAbility.OnEntityRequested(pos, PlayerAbility.TextureString, PlayerAbility.GetType()) as SpecialAbilitySlug);

                    // get slug to chase
                    ((SpecialAbilitySlug)mAbilitiesActive.Last()).Run(pathFinding.mGrid, pos, FindClosestPlayer(pPlayer));
                }
            }
        }

        /// <summary>
        /// Summon ice slows a player down
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pDirX"></param>
        /// <param name="pDirY"></param>
        void SummonIce(Player pPlayer, int pDirX, int pDirY)
        {
            Vector2 pos = pPlayer.Position;

            PlaceBlock(pos, pDirX, pDirY);

            mAbilitiesActive.Add(PlayerAbility.OnEntityRequested(PlayerAbility.Position, PlayerAbility.TextureString, PlayerAbility.GetType()) as SpecialAbilityIce);

            SetWalkableArea();
        }

        /// <summary>
        /// Summon wall blocks player
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pDirX"></param>
        /// <param name="pDirY"></param>
        void SummonWall(Player pPlayer, int pDirX, int pDirY)
        {
            Vector2 pos = pPlayer.Position;

            PlaceBlock(pos, pDirX, pDirY);

            mAbilitiesActive.Add(PlayerAbility.OnEntityRequested(PlayerAbility.Position, PlayerAbility.TextureString, PlayerAbility.GetType()) as SpecialAbilityWall);

            SetWalkableArea();
        }

        /// <summary>
        /// Set the position of the block
        /// </summary>
        /// <param name="pPos"></param>
        /// <param name="pDirX"></param>
        /// <param name="pDirY"></param>
        void PlaceBlock(Vector2 pPos, int pDirX, int pDirY)
        {
            int wallDisPlacement = 80;

            if (pDirY > 0)
            {
                PlayerAbility.Position = new Vector2(pPos.X, pPos.Y - wallDisPlacement);
            }
            else if (pDirY < 0)
            {
                PlayerAbility.Position = new Vector2(pPos.X, pPos.Y + wallDisPlacement);
            }
            else if (pDirX > 0)
            {
                PlayerAbility.Position = new Vector2(pPos.X - wallDisPlacement, pPos.Y);
            }
            else
            {
                PlayerAbility.Position = new Vector2(pPos.X + wallDisPlacement, pPos.Y);
            }
        }

        /// <summary>
        /// Set the walkable areas on path finding
        /// </summary>
        void SetWalkableArea()
        {
            pathFinding.mGrid.SetWalkable(mAbilitiesActive.Last().Position, false);
        }

        /// <summary>
        /// Find the closest player for the slug
        /// </summary>
        /// <param name="pPlayer">Who called it</param>
        /// <returns></returns>
        Player FindClosestPlayer(Player pPlayer)
        {
            float distanceToClosestPlayer = float.PositiveInfinity;

            Player closestPlayer = null;

            // put all players in a list
            List<Player> allPlayers = mPlayers.Values.ToList();
            // remove the one who called it
            allPlayers.Remove(pPlayer);

            // loop through and find who is the closest player
            foreach (var p in allPlayers.ToList())
            {
                float distance = Vector2.Distance(p.Position, pPlayer.Position);

                if (distance < distanceToClosestPlayer)
                {
                    closestPlayer = p;
                    distanceToClosestPlayer = distance;
                }
            }

            return closestPlayer;

        }

        /// <summary>
        /// Destroy objects
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pNum"></param>
        public void CallDestroy(Player pPlayer, int pNum)
        {
            if (mPlayers.ContainsValue(pPlayer))
            {
                if (mAbilitiesActive.Count() - 1 <= pNum)
                {
                    Destroy(pNum);
                }
            }
        }

        /// <summary>
        /// Destroy the ability
        /// </summary>
        /// <param name="num"></param>
        void Destroy(int num)
        {
            // if its not slug then delete slug will delete itself
            if (mAbilitiesActive[num].GetType() != typeof(SpecialAbilitySlug))
            {
                mAbilitiesActive[num].Destroy(mAbilitiesActive[num]);
                pathFinding.mGrid.SetWalkable(mAbilitiesActive[num].Position, true);
                mAbilitiesActive.RemoveAt(num);
            }
        }

        /// <summary>
        /// Clear static when starting a new level
        /// </summary>
        public void ClearStaticPlayers()
        {
            mPlayers.Clear();
        }
    }
}

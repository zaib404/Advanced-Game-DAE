using Engine.Collision;
using Engine.Entity;
using GameCode.Entities;
using GameCode.Misc;
using System.Collections.Generic;

namespace GameCode.Special_Abilities
{
    public class SpecialAbilityIce : GameEntity, ISpecialAbility, iCollidable
    {
        // keep a list a players
        List<Player> players = new List<Player>();

        public SpecialAbilityIce()
        {
            // set texture
            TextureString = @"Abilities\IceAbility";

            // grab verticies
            Vertices = ReadFile.GetVerticiesFromTSX(TextureString);
            // set true
            IsTrigger = true;
            DrawPriority = 2;
        }

        public override void OnTriggerEnter(iEntity gameObject)
        {
            // if its player
            if (gameObject is Player p)
            {
                if (!players.Contains(p))
                {
                    // add to list
                    players.Add(p);
                    // half the acceleration
                    p.Acceleration /= 2;
                }
            }
        }

        public override void OnTriggerExit(iEntity gameObject)
        {
            if (gameObject is Player p)
            {
                if (players.Contains(p))
                {
                    // speed return to normal
                    p.Acceleration *= 2;
                    players.Remove(p);
                }
            }
        }

        public override void Destroy(iEntity ent)
        {
            // return each players acceleration back to normal
            foreach (var player in new List<Player>(players))
            {
                player.Acceleration *= 2;
                players.Remove(player);
            }
            base.Destroy(ent);
        }
    }
}

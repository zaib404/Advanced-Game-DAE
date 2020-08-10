using Engine.Collision;
using Engine.Entity;
using GameCode.Misc;

namespace GameCode.Special_Abilities
{
    public class SpecialAbilityWall : GameEntity, ISpecialAbility, iCollidable
    {
        public SpecialAbilityWall()
        {
            // set texture
            TextureString = @"Abilities\WallAbility2";

            // get Vertices from tsx file
            Vertices = ReadFile.GetVerticiesFromTSX(TextureString);
            DrawPriority = 2;
        }
    }
}

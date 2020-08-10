using Game1.Engine.Pathfinding;
using GameCode.Entities;

namespace GameCode.Special_Abilities
{
    public interface ISpecialAbilityManager
    {
        ISpecialAbility PlayerAbility { get; }
        void GiveAbilty(Player pPlayer, IGrid pGrid);
        void CallSpecialAbilty(Player pPlayer, int x, int y);
        void CallDestroy(Player pPlayer, int pNum);
        void ClearStaticPlayers();
    }
}

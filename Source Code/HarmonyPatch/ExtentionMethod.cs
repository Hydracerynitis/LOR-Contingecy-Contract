using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    static class ExtentionMethod
    {
        public static List<BattlePlayingCardDataInUnitModel> triggeredCard = new List<BattlePlayingCardDataInUnitModel>();
        public static void TriggerTagTeam(this TagTeam ability, Faction f, LorId targetCard)
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(f))
            {
                if (unit.cardSlotDetail.cardAry.Find(x => x != null && x.card.GetID() == targetCard && !triggeredCard.Contains(x)) is BattlePlayingCardDataInUnitModel page)
                {
                    ability.TagTeamEffect(unit);
                    triggeredCard.Add(page);
                    return;
                }
            }
        }
        public static bool GreaterEqual(this LorId lhs, int rhs)
        {
            return lhs.IsBasic() && lhs.id >= rhs;
        }
        public static bool LesserEqual(this LorId lhs, int rhs)
        {
            return lhs.IsBasic() && lhs.id <= rhs;
        }
    }
    public interface TagTeam
    {
        void TagTeamEffect(BattleUnitModel tagTeamMate);
    }
    public interface Retaliater
    {
        BattlePlayingCardDataInUnitModel Retaliate(BattlePlayingCardDataInUnitModel attackerCard);
    }
    public interface GetRecovery
    {
        int GetRecoveryBonus(int v);
    }
    public interface Resonator
    {
        void ActiveResonate(BattlePlayingCardDataInUnitModel card);
    }
    public interface CardDrawer
    {
        int getDrawCardAdder(int userCard);
    }
    public class BattleUnitBuf_Extention: BattleUnitBuf
    {
        public virtual void OnStartBattle()
        {

        }
    }
}

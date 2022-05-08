using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    static class ExtentionMethod
    {
        public static void TriggerTagTeam(this TagTeam ability, Faction f, LorId targetCard)
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(f))
            {
                if (unit.cardSlotDetail.cardAry.Exists(x => x != null && x.card.GetID() == targetCard))
                {
                    ability.TagTeamEffect(unit);
                    return;
                }
            }
        }
        public static void TriggerTagTeam(this TagTeam ability, Faction f, params LorId[] ids)
        {
            List<LorId> idList = new List<LorId>(ids);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(f))
            {
                if (unit.cardSlotDetail.cardAry.Exists(x => idList.Contains(x.card.GetID())))
                {
                    ability.TagTeamEffect(unit);
                    return;
                }
            }
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
}

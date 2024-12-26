using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    static class ExtentionMethod
    {
        public static List<BattlePlayingCardDataInUnitModel> triggeredTagTeamCard = new List<BattlePlayingCardDataInUnitModel>();
        public static void TriggerTagTeam(this TagTeam ability, Faction f, LorId targetCard)
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(f))
            {
                if (unit.cardSlotDetail.cardAry.Find(x => x != null && x.card.GetID() == targetCard && !triggeredTagTeamCard.Contains(x)) is BattlePlayingCardDataInUnitModel page)
                {
                    ability.TagTeamEffect(unit);
                    triggeredTagTeamCard.Add(page);
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
    public interface StartBattleBuf
    {
        void OnStartBattle();
    }
    public interface OnStandBy
    {
        void OnStandBy(BattlePlayingCardDataInUnitModel card, BattleUnitModel unit, List<BattleDiceBehavior> StandByDie);
    }
    public interface RecoverHpBuf
    {
        void OnRecoverHp(int amount);
    }
}

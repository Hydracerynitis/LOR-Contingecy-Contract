using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoKeywordUtil;

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
        public static void AddAutoBufByCard<T>(this BattleUnitBufListDetail unitBufListDetail, int stack, BattleUnitModel actor = null, BufReadyType readyType = BufReadyType.ThisRound)
      where T : IAutoKeywordBuf
        {
            
            switch (readyType)
            {
                case BufReadyType.ThisRound:
                    unitBufListDetail.AddKeywordBufThisRoundByCard(AutoKeywordUtils.GetAutoKeyword(typeof(T)), stack, actor);
                    return;
                case BufReadyType.NextRound:
                    unitBufListDetail.AddKeywordBufByCard(AutoKeywordUtils.GetAutoKeyword(typeof(T)), stack, actor);
                    return;
                case BufReadyType.NextNextRound:
                    unitBufListDetail.AddKeywordBufNextNextByCard(AutoKeywordUtils.GetAutoKeyword(typeof(T)), stack, actor);
                    return;
            }
        }

        public static void AddAutoBufByEtc<T>(this BattleUnitBufListDetail unitBufListDetail, int stack, BattleUnitModel actor = null, BufReadyType readyType = BufReadyType.ThisRound)
      where T : IAutoKeywordBuf
        {

            switch (readyType)
            {
                case BufReadyType.ThisRound:
                    unitBufListDetail.AddKeywordBufThisRoundByEtc(AutoKeywordUtils.GetAutoKeyword(typeof(T)), stack, actor);
                    return;
                case BufReadyType.NextRound:
                    unitBufListDetail.AddKeywordBufByEtc(AutoKeywordUtils.GetAutoKeyword(typeof(T)), stack, actor);
                    return;
                case BufReadyType.NextNextRound:
                    return;
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
        void OnStartBattle(BattleUnitModel unit);
    }
    public interface StartBattleInHandBuf
    {
        void OnStartBattle_inHand(BattleUnitModel unit);
    }
    public interface OnStandBy
    {
        void OnStandBy(BattlePlayingCardDataInUnitModel card, BattleUnitModel unit, List<BattleDiceBehavior> StandByDie);
    }
    public interface RecoverHpBuf
    {
        void OnRecoverHp(int amount);
    }
    public interface OnUseOtherCardInHand
    {
        void OnUseOtherCardInHand(BattleUnitModel unit, BattlePlayingCardDataInUnitModel card);
    }
    public interface StaggerDamageReductionAllBuf
    {
        int GetBreakDamageReductionAll(int dmg, DamageType dmgType, BattleUnitModel attacker);
    }
    public interface OnAddToHandBuf
    {
        void OnAddToHand(BattleUnitModel unit);
    }
}

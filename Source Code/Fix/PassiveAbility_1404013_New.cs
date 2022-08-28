using System.Collections.Generic;

namespace Fix
{
    public class PassiveAbility_1404013_New : PassiveAbilityBase
    {
        public override void OnRoundStart() => this.owner.emotionDetail.SetMaxEmotionLevel(0);

        public override void OnDie()
        {
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy);
            int stack = 1;
            if (aliveList.Count == 0)
                return;
            foreach (BattleUnitModel battleUnitModel in aliveList)
            {
                battleUnitModel.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, stack, this.owner);
                battleUnitModel.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, stack, this.owner);
                battleUnitModel.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, stack, this.owner);
            }
        }
    }
}
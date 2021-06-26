using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_MeatGearSmoke: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return !owner.bufListDetail.GetActivatedBufList().Exists(x=> x is BattleUnitBuf_ZealLimit);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new BattleUnitBuf_ZealLimit());
            unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke,2);
            unit.personalEgoDetail.RemoveCard(18200008);
            base.OnUseInstance(unit, self, targetUnit);
        }
    }
}

using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_MeatGearPower: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return !owner.bufListDetail.GetActivatedBufList().Exists(x=> x is BattleUnitBuf_ZealLimit);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new BattleUnitBuf_ZealLimit());
            unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.AllPowerUp,1);
            unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(18200006));
            base.OnUseInstance(unit, self, targetUnit);
        }
    }
}

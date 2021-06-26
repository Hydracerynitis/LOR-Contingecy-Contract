using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_MeatGearHeal: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return !owner.bufListDetail.GetActivatedBufList().Exists(x=> x is BattleUnitBuf_ZealLimit);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new BattleUnitBuf_ZealLimit());
            unit.RecoverHP(10);
            unit.breakDetail.RecoverBreak(10);
            unit.personalEgoDetail.RemoveCard(18200007);
            base.OnUseInstance(unit, self, targetUnit);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_SacrificeHp : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.hp>owner.MaxHp/4;
        }
        public override bool IsOnlyAllyUnit()
        {
            return true;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            int hp = unit.MaxHp / 4;
            unit.LoseHp(hp);
            targetUnit.RecoverHP(hp / 2);
        }
    }
}

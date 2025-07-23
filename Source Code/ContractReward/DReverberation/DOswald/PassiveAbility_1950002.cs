using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1950002 : PassiveAbilityBase
    {
        private int minHp = -1;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            minHp = (int)owner.hp - owner.MaxHp / 2;
        }
        public override int GetMinHp()
        {
            return minHp;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction == owner.faction)
                owner.breakDetail.TakeBreakDamage(20);
        }
    }
}

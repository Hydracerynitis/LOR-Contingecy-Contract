using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1850001 : PassiveAbilityBase
    {
        public override int GetBreakDamageReductionAll( int dmg,  DamageType dmgType, BattleUnitModel attacker)
        {
            int num = 0;
            if (dmgType == DamageType.Attack || dmgType == DamageType.Rebound)
                num = dmg / 2;
            return num;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.faction == owner.faction)
                owner.TakeBreakDamage(30, DamageType.Passive);
        }
    }
}

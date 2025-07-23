using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880001: PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            this.owner.RecoverHP(4);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (IsDefenseDice(behavior.Detail))
                return;
            double ratio = 1-this.owner.hp / this.owner.MaxHp;
            int count = 0;
            for(;ratio>=0.25 && count < 3; count++)
            {
                ratio -= 0.25;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = count });
        }
    }
}

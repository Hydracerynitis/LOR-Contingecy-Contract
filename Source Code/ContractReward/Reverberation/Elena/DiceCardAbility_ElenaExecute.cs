using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    class DiceCardAbility_ElenaExecute: DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (behavior.card.target == null)
                return;
            base.BeforeRollDice();
            if (behavior.TargetDice != null)
                behavior.TargetDice.AddAbility(new DiceCardAbility_invalid());
            this.behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = (int)behavior.card.target.hp });
        }
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            this.behavior.card.target.breakDetail.LoseBreakLife();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    class DiceCardAbility_ElenaExecute: DiceCardAbilityBase
    {
        public override void BeforRollDice()
        {
            base.BeforRollDice();
            if (behavior.TargetDice != null)
                behavior.TargetDice.AddAbility(new DiceCardAbility_invalid());
        }
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            this.behavior.card.target.breakDetail.LoseBreakLife();
        }
    }
}

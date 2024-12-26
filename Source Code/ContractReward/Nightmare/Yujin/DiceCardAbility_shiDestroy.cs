using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_shiDestroy : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (behavior.DiceResultValue >= 20)
            {
                behavior.card.target.currentDiceAction?.DestroyDice(DiceMatch.AllDice, DiceUITiming.AttackAfter);
            }
        }
    }
}

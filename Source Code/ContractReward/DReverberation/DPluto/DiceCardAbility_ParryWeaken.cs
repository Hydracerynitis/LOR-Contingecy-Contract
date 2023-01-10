using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_ParryWeaken : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            if (behavior.TargetDice != null)
                behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = -35, dmgRate=-35 });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_FocusClash : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            Focus.AddStack(owner, 2);
        }
    }
}

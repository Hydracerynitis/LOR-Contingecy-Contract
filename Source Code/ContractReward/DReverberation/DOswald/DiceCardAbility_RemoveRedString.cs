using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_RemoveRedString : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_RedString));
        }
    }
}

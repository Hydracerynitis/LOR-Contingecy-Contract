using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;

namespace ContractReward
{
    public class DiceCardAbility_JaheonStringAtk : DiceCardAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            target.bufListDetail.AddBufByCard<BattleUnitBuf_JaeheonControl>(1, readyType: BufReadyType.NextRound);
        }
    }
}

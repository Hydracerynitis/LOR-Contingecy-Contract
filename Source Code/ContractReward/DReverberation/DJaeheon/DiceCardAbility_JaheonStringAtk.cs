using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using Contingecy_Contract;

namespace ContractReward
{
    public class DiceCardAbility_JaheonStringAtk : DiceCardAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            target.bufListDetail.AddAutoBufByCard<BattleUnitBuf_JaeheonControl>(1, readyType: BufReadyType.NextRound);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using static UnityEngine.GraphicsBuffer;

namespace ContractReward
{
    public class DiceCardAbility_StealRecovery : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            BattleUnitBuf buf = new BattleUnitBuf_RecoveryHijack(owner);
            card.target.bufListDetail.AddReadyBuf(buf);
        }
    }
}

using BaseMod;
using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_RiLight : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana4>())
            {
                owner.cardSlotDetail.RecoverPlayPoint(3);
            }
        }
    }
}

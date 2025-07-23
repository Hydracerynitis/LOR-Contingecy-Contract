using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510007 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            if (this.owner.cardSlotDetail.PlayPoint > 0)
                return;
            this.owner.cardSlotDetail.RecoverPlayPoint(2);
            Focus.AddStack(owner, 3);
        }
    }
}

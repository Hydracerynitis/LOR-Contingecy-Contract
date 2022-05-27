using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contingecy_Contract;

namespace ContractReward
{
    public class PassiveAbility_1800001 : PassiveAbilityBase, Resonator
    {
        public void ActiveResonate(BattlePlayingCardDataInUnitModel card)
        {
            this.owner.allyCardDetail.DrawCards(2);
            this.owner.cardSlotDetail.RecoverPlayPoint(3);
        }
    }
}

using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510002 : PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.card.GetOriginCost() == 3)
                return;
            this.owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}

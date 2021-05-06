using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880002: PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.personalEgoDetail.AddCard(18800007);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card.GetID() == 18800007)
                this.owner.personalEgoDetail.RemoveCard(18800007);
        }
    }
}

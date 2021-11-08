using BaseMod;
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
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18800006));
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card.GetID() == Tools.MakeLorId(18800006))
                this.owner.personalEgoDetail.RemoveCard(Tools.MakeLorId(18800006));
        }
    }
}

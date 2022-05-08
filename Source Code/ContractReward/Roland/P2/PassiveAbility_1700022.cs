using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700022 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.allyCardDetail.DrawCards(2);
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            card.target.breakDetail.LoseBreakGauge(card.target.breakDetail.breakGauge/5);
        }
    }
}

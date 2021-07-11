using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_energy3 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[1]
        {
            "Energy_Keyword"
        };
        public override void OnUseCard() => this.owner.cardSlotDetail.RecoverPlayPointByCard(3);
    }
}

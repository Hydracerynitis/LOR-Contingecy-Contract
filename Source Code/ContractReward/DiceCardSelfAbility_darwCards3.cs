using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_drawCards3 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard() => this.owner.allyCardDetail.DrawCards(3);
    }
}

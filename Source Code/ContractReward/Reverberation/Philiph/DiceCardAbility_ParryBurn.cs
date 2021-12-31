using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_ParryBurn : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            if (this.behavior.IsParrying())
            {
                this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 1,owner);
                behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 1, owner);
            }
        }
    }
}

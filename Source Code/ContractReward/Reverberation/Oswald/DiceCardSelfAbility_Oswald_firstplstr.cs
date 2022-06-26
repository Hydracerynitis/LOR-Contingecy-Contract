using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_firstplstr : DiceCardSelfAbilityBase
    {
        private int loseCount;

        public override void OnLoseParrying()
        {
            if (this.loseCount != 0)
                return;
            ++this.loseCount;
            this.card?.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, this.owner);
        }
    }
}

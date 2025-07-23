using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaHaste : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness,2, this.owner);
            if (this.card.speedDiceResultValue < 7)
                return;
            this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = 2
            });
        }
    }
}


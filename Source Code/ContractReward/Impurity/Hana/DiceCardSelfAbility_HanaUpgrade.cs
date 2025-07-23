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
    public class DiceCardSelfAbility_HanaUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnWinParryingAtk()
        {
            card.AddDiceMaxValue(DiceMatch.NextDice, 3);
        }
        public override void OnWinParryingDef()
        {
            card.AddDiceMaxValue(DiceMatch.NextDice, 3);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_SpeedMin : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            int speed = card.speedDiceResultValue;
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = Math.Min(speed / 2, 4) });
        }
    }
}

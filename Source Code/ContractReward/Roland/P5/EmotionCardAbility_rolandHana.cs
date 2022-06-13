using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandHana : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            int num1 = 2;
            int num2 = 0;
            if (_owner.emotionDetail.EmotionLevel >= 3)
                num2 = 1;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = num1,
                max = num2
            });
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            behavior.card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus() { max = 2 });
        }
    }
}

using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandShi : EmotionCardAbilityBase
    {
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (behavior.DiceVanillaValue > 5 && behavior.DiceVanillaValue == behavior.GetDiceMax())
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 4 });
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}

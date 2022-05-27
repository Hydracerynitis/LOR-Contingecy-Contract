using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_PerfectResonanceActivate : DiceCardSelfAbilityBase
    {
        public override void OnActivateResonance()
        {
            base.OnActivateResonance();
            if(card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Vibrate)==card.speedDiceResultValue)
                card.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() { power = 2 });
        }
    }
}

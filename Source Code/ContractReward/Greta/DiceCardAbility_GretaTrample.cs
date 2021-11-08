using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_GretaTrample : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1]
        {
            "Recover_Keyword"
        };
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            BattleUnitModel target = this.card.target;
            if (target == null || target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding) == null)
                return;
            BattleDiceBehavior behavior = this.behavior;
            if (behavior == null)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 2
            });
        }
    }
}

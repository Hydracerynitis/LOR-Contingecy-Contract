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
    public class DiceCardSelfAbility_PlutoAoe : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnUseAoe()
        {
            base.OnUseAoe();
            int count = this.card.subTargets.Count + 1;
            this.card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { dmg = Math.Max((5 - count) * 3,0) });
        }
    }
}

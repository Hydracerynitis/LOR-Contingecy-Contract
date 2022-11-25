using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RecoverEnhance : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnUseAoe()
        {
            base.OnUseAoe();
            card.ApplyDiceStatBonus(x => x.index == 1, new DiceStatBonus() { max = 2 * owner.UnitData.historyInWave.healed / 20 }); 
        }
    }
}

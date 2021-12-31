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
    public class DiceCardSelfAbility_GretaMeatMeal : DiceCardSelfAbilityBase
    {
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return targetUnit.bufListDetail.HasBuf<BattleUnitBuf_FreshMeat>();
        }
    }
}

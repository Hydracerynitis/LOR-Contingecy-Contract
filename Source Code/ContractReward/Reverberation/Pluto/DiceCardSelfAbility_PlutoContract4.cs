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
    public class DiceCardSelfAbility_PlutoContract4 : DiceCardSelfAbilityBase
    {
        public override bool IsTargetableAllUnit() => true;
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            if (targetUnit.faction == unit.faction)
                targetUnit.bufListDetail.AddBuf(new BattleUnitBuf_Pluto_Contracted_4());
            else 
                unit.bufListDetail.AddBuf(new BattleUnitBuf_Pluto_Contracted_4());
            unit.personalEgoDetail.RemoveCard(self.GetID());
        }
    }
}

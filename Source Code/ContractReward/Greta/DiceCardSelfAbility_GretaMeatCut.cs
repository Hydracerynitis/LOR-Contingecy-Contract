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
    public class DiceCardSelfAbility_GretaMeatCut : DiceCardSelfAbility_energy3
    {
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return targetUnit.bufListDetail.HasBuf<BattleUnitBuf_FreshMeat>();
        }
        private bool HasTranformed(BattleDiceCardModel card) => typeof(BattleDiceCardModel).GetField("_originalXmlData", AccessTools.all).GetValue(card)==null;
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if(BattleObjectManager.instance.GetAliveList_opponent(unit.faction).Exists(x => x.bufListDetail.HasBuf<BattleUnitBuf_FreshMeat>()))
                return;
            if (HasTranformed(self))
                self.ResetToOriginalData();
            else
            {
                DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(18300004);
                typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(self, xml);
            }
            typeof(BattleDiceCardModel).GetField("_script", AccessTools.all).SetValue(self, self.CreateDiceCardSelfAbilityScript());
        }
    }
}

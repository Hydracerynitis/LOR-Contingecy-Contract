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
    public class DiceCardSelfAbility_GretaCut : DiceCardSelfAbility_energy3
    {
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if(BattleObjectManager.instance.GetAliveList_opponent(unit.faction).Exists(x => x.bufListDetail.HasBuf<BattleUnitBuf_FreshMeat>()))
            {
                self.CopySelf();
                DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(18300012);
                typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(self, xml);
            }
            typeof(BattleDiceCardModel).GetField("_script", AccessTools.all).SetValue(self, self.CreateDiceCardSelfAbilityScript());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;
using System.Text;
using LOR_DiceSystem;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_GretaCut : DiceCardSelfAbility_energy3
    {
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if(BattleObjectManager.instance.GetAliveList_opponent(unit.faction).Exists(x => x.bufListDetail.HasBuf<BattleUnitBuf_FreshMeat>()))
            {
                DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(18300012));
                self._xmlData = xml;
                self._script = new DiceCardSelfAbility_GretaMeatCut();
            }
        }
    }
}

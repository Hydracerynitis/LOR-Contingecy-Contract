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
    public class DiceCardSelfAbility_strength2quickness2friend : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            BattleUnitModel unit=BattleObjectManager.instance.GetAliveList_random(owner.faction, 1)[0];
            unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2,owner);
            unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2,owner);
        }
    }
}

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
    public class DiceCardSelfAbility_AttackTarget : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            foreach(BattleUnitModel Unit in BattleObjectManager.instance.GetAliveList_opponent(unit.faction))
            {
                if (Unit.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonMark) != null)
                    Unit.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonMark).Destroy();
            }
            targetUnit.bufListDetail.AddBuf(new BattleUnitBuf_AttackTarget());
        }
    }
}

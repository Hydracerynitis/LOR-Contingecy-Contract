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
    public class DiceCardSelfAbility_AllVibrate : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnUseAoe()
        {
            base.OnUseAoe();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                BattleUnitBuf activatedBuf = unit.bufListDetail.GetActivatedBuf(KeywordBuf.Vibrate);
                if (activatedBuf != null)
                    activatedBuf.stack = 4;
                else
                    unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Vibrate, 4);
            }
        }
    }
}

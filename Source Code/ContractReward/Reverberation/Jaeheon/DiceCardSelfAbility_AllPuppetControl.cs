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
    public class DiceCardSelfAbility_AllPuppetControl : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnUseAoe()
        {
            base.OnUseAoe();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                if (unit == this.owner)
                    continue;
                unit.bufListDetail.AddReadyBuf(new BattleUnitBuf_AngelicaPuppet());
            }
        }
    }
}

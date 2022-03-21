using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace Fix
{
    public class DiceCardSelfAbility_Jaeheon_AreaDt_New : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                if (!alive.UnitData.unitData.EnemyUnitId.IsBasic())
                    continue;
                switch (alive.UnitData.unitData.EnemyUnitId.id)
                {
                    case 1307021:
                    case 1307031:
                    case 1307041:
                    case 1307051:
                        BattleUnitBuf_Jaeheon_PuppetThread jaeheonPuppetThread = new BattleUnitBuf_Jaeheon_PuppetThread();
                        jaeheonPuppetThread.Init(alive);
                        jaeheonPuppetThread.stack = 1;
                        alive.bufListDetail.AddReadyBuf(jaeheonPuppetThread);
                        continue;
                    default:
                        continue;
                }
            }
        }
    }
}

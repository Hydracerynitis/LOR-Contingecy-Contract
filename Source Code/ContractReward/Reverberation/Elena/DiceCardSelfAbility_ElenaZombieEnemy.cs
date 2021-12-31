using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_ElenaZombieEnemy: DiceCardSelfAbility_ElenaZombie
    {
        public override bool IsTargetChangable(BattleUnitModel attacker)
        {
            return false;
        }
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return true;
        }        
    }
}

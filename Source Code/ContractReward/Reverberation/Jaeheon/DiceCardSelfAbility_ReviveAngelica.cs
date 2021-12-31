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
    public class DiceCardSelfAbility_ReviveAngelica : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return BattleObjectManager.instance.GetFriendlyAllList(owner.faction).Find(x => x.IsDead()==true)!=null;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1870001) is PassiveAbility_1870001 passive)
                passive.Revive();
            self.exhaust=true;
            base.OnUseInstance(unit, self, targetUnit);
        }
    }
}

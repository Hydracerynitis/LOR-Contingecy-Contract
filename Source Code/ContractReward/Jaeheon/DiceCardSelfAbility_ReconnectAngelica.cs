using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_ReconnectAngelica : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (FindAngelica(unit) != null)
                FindAngelica(unit).bufListDetail.AddBuf(new BattleUnitBuf_AngelicaPuppet(unit));
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1870001) is PassiveAbility_1870001 passive)
                passive.ReturnToPassive();
            self.exhaust = true;
            base.OnUseInstance(unit, self, targetUnit);
        }
        private BattleUnitModel FindAngelica(BattleUnitModel unit) => BattleObjectManager.instance.GetAliveList(unit.faction).Find(x => x.Book.GetBookClassInfoId() == Tools.MakeLorId(18710000));
    }
}

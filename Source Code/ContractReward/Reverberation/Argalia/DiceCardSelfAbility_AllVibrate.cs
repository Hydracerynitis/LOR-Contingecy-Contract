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
    public class DiceCardSelfAbility_AllVibrate : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((x => x is CoolDown)) == null;
        }
        public override void OnUseCard()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                BattleUnitBuf activatedBuf = unit.bufListDetail.GetActivatedBuf(KeywordBuf.Vibrate);
                if (activatedBuf != null)
                    activatedBuf.stack = 4;
                else
                    unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Vibrate, 4);
            }
            this.owner.bufListDetail.AddBuf(new CoolDown());
        }
        public class CoolDown : BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 4;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                stack--;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}

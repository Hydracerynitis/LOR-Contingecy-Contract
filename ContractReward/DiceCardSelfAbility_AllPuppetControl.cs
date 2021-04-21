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
    public class DiceCardSelfAbility_AllPuppetControl : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DiceCardSelfAbility_AllPuppetControl.CoolDown)) == null;
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                if (unit == this.owner)
                    continue;
                unit.bufListDetail.AddReadyBuf(new BattleUnitBuf_AngelicaPuppet());
            }
            this.owner.bufListDetail.AddBuf(new DiceCardSelfAbility_AllPuppetControl.CoolDown());
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

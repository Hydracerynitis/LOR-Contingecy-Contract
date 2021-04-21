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
    public class DiceCardSelfAbility_PlutoAoe : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((x => x is DiceCardSelfAbility_PlutoAoe.CoolDown)) == null;
        }
        public override void OnUseCard()
        {
            int count = this.card.subTargets.Count + 1;
            this.card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { dmg = (5 - count) * 3 });
            this.owner.bufListDetail.AddBuf(new DiceCardSelfAbility_AllVibrate.CoolDown());
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

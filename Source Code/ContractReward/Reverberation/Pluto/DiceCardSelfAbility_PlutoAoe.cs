using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using BaseMod;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_PlutoAoe : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((x => x is CoolDown)) == null;
        }
        public override void OnUseCard()
        {
            int count = this.card.subTargets.Count + 1;
            this.card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { dmg = Math.Max((5 - count) * 3, 0) });
            this.owner.bufListDetail.AddBuf(new CoolDown());
        }
        public class CoolDown : BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 2;
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

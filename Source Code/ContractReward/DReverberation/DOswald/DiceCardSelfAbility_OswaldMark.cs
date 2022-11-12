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
    public class DiceCardSelfAbility_OswaldMark : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            card.target.bufListDetail.AddBuf(new OswaldMark());
        }
        class OswaldMark: BattleUnitBuf
        {
            public override string keywordId => "OswaldMark";
            public override string keywordIconId => "Oswald_Daze";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override int GetDamageIncreaseRate()
            {
                return 50;
            }
            public override int GetBreakDamageIncreaseRate()
            {
                return 50;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

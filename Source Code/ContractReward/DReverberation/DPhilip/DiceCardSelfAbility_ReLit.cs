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
    public class DiceCardSelfAbility_ReLit : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new BurnPlus());
        }
        class BurnPlus: BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                return cardBuf is BattleUnitBuf_burn ? 2 : 0;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using BaseMod;
using Contingecy_Contract;

namespace ContractReward
{
    public class DiceCardSelfAbility_JaheonString : DiceCardSelfAbilityBase
    {
        private bool hit = false;
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            hit = true;
        }
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (hit)
                card.target.bufListDetail.AddAutoBufByCard<BattleUnitBuf_JaeheonControl>(1, readyType: BufReadyType.NextRound);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Insane1 : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Insanity) is BattleUnitBuf_Insanity insanity)
            {
                if (insanity.stack > 200)
                    return true;
                else
                    return insanity.stack > 100 && !owner.cardSlotDetail.cardAry.Exists(x => x!= null && (x.card.GetID().id== 18000011 || x.card.GetID().id == 18000012));
            }
            else
                return false;
        }
        public override void OnUseCard()
        {
            BattleUnitBuf insanity = this.owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Insanity);
            insanity.stack -= 100;
            this.card.target.bufListDetail.AddReadyBuf(new NoPower());
        }
        public class NoPower : BattleUnitBuf
        {
            public override string keywordId => "NullifyPowerAll";
            public override string keywordIconId => "NullifyPower";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override bool IsNullifiedPower() => true;
        }
    }
}

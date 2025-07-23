using LOR_DiceSystem;
using Contingecy_Contract;
using System.Collections.Generic;
using HP = SummonLiberation.Harmony_Patch;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using System;
using UnityEngine;
using AutoKeywordUtil;

namespace ContractReward
{
    public class UnjustProtection : BattleUnitBuf, IAutoKeywordBuf
    {
        public override string keywordIconId => "PlutoUnfairProtect";
        public override string keywordId => "UnjustProctect";
        public string KeywordBufName => "CC_UnjustProctect";
        public override KeywordBuf bufType => AutoKeywordUtils.GetAutoKeyword(typeof(UnjustProtection));
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 3);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddBuf(new UnjustProtectionDividend());
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --stack;
            if (stack <= 0)
                Destroy();
        }
    }
    public class DiceCardSelfAbility_UnjustProctect : DiceCardSelfAbilityBase
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
                card.target.bufListDetail.AddAutoBufByCard<UnjustProtection>(2, readyType: BufReadyType.NextRound);
        }
    }
    public class UnjustProtectionDividend : BattleUnitBuf
    {
        public override string keywordIconId => "UnfairProtect";
        public override string keywordId => "UnjustProctectDividend";
        public override int GetBreakDamageReductionRate()
        {
            return 50;
        }
        public override int GetDamageReductionRate()
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

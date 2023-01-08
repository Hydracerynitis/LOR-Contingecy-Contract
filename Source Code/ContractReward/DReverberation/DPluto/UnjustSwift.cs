using LOR_DiceSystem;
using Contingecy_Contract;
using System.Collections.Generic;
using HP = SummonLiberation.Harmony_Patch;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using System;
using UnityEngine;

namespace ContractReward
{
    public class UnjustSwift: BattleUnitBuf
    {
        public override string keywordIconId => "PlutoUnfairLight";
        public override string keywordId => "UnjustSwift";
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 3);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddBuf(new UnjustSwiftDividend());
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --stack;
            if (stack <= 0)
                Destroy();
        }
    }
    public class DiceCardSelfAbility_UnjustSwift : DiceCardSelfAbilityBase
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
                card.target.bufListDetail.AddBufByCard<UnjustSwift>(2, readyType: BufReadyType.NextRound);
        }
    }
    public class UnjustSwiftDividend : BattleUnitBuf
    {
        public override string keywordIconId => "UnfairLight";
        public override string keywordId => "UnjustSwiftDividend";
        public override int SpeedDiceNumAdder()
        {
            return 2;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            Destroy();
        }
    }
}

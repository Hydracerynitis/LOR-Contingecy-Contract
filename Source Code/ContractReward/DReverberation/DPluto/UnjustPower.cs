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
    public class UnjustPower : BattleUnitBuf
    {
        public override string keywordIconId => "PlutoUnfairAtk";
        public override string keywordId => "UnjustPower";
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction)).bufListDetail.AddBuf(new UnjustPowerDividend());
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --stack;
            if (stack <= 0)
                Destroy();
        }
    }
    public class DiceCardSelfAbility_UnjustPower : DiceCardSelfAbilityBase
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
                card.target.bufListDetail.AddBufByCard<UnjustPower>(2, readyType: BufReadyType.NextRound);
        }
    }
    public class UnjustPowerDividend: BattleUnitBuf
    {
        public override string keywordIconId => "UnfairAtk";
        public override string keywordId => "UnjustPowerDividend";
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            this.stack = RandomUtil.SelectOne(0, 1, 2, 3);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            BattleDiceCardModel card = behavior.card?.card;
            if (card == null)
                return;
            if (card.GetCost() == this.stack)
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 3
                });
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            Destroy();
        }
    }
}

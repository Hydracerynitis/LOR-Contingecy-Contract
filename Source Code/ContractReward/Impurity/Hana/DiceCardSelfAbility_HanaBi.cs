using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaBi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new BiBuff();
        }
        class BiBuff : HexagramCardBuf, OnUseOtherCardInHand
        {
            public override string keywordIconId => "比Buf";
            public override string keywordId => "HanaBi";
            public override int paramInBufDesc => staggerShield;
            private int staggerShield => 3 + _card.GetCost() * 6;
            public BiBuff()
            {
                _stack = 0;
            }
            public void OnUseOtherCardInHand(BattleUnitModel unit, BattlePlayingCardDataInUnitModel card)
            {
                if (card.card.GetCost() > _card.GetCost())
                    _stack++;
            }
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                owner.bufListDetail.AddBuf(new StaggerShield(staggerShield * _stack));
                _stack = 0;
            }
        }
    }
    public class StaggerShield: BattleUnitBuf, StaggerDamageReductionAllBuf
    {
        public override string keywordId => "staggerShield";
        public StaggerShield(int _stack)
        {
            stack = _stack;
        }
        public int GetBreakDamageReductionAll(int dmg, DamageType dmgType, BattleUnitModel attacker)
        {
            int absorbAmount = Math.Min(dmg, stack);
            stack -= absorbAmount;
            if(stack <= 0)
                Destroy();
            return absorbAmount;
        }
    }
}

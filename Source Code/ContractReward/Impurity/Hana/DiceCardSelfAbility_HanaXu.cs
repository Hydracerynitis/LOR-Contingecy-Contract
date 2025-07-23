using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaXu : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new XuBuff();
        }
        class XuBuff : HexagramCardBuf, OnUseOtherCardInHand
        {
            public override string keywordIconId => "需Buf";
            public override string keywordId => "HanaXu";
            public override int paramInBufDesc => additionalDamage;
            private int additionalDamage => 2 + _card.GetCost() * 4;
            public XuBuff()
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
                playingCard.target.TakeBreakDamage(Stack * additionalDamage);
                _stack = 0;
            }
        }
    }
}

using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaJiJi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new JiJiBuf();
        }
        class JiJiBuf : HexagramCardBuf, OnUseOtherCardInHand
        {
            public override string keywordIconId => "既济Buf";
            public override string keywordId => "HanaJiJi";
            public override int paramInBufDesc => _card.GetCost();
            public void OnUseOtherCardInHand(BattleUnitModel unit, BattlePlayingCardDataInUnitModel card)
            {
                if (card.card.GetCost() > _card.GetCost())
                    _card.AddCost(-1);
            }
        }
    }
}

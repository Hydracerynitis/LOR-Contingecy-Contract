using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaSong : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new SongBuff();
        }
        class SongBuff: HexagramCardBuf
        {
            private int trigger = 0;
            public override string keywordIconId => "讼Buf";
            public override string keywordId => "HanaSong";
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                if (trigger != 0)
                    return;
                playingCard.cardAbility?.OnUseCard();
                trigger = 1;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                trigger = 0;
            }
        }
    }
}

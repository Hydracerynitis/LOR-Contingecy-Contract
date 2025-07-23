using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaTongRen : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new TongRenBuff();
        }
        class TongRenBuff : HexagramCardBuf,StartBattleBuf
        {
            private int CopyCount = 0;
            public override string keywordIconId => "同人Buf";
            public override string keywordId => "HanaTongRen";
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                playingCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 * CopyCount });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                CopyCount = 0;
            }

            public void OnStartBattle(BattleUnitModel unit)
            {
                foreach(BattlePlayingCardDataInUnitModel page in unit.cardSlotDetail.cardAry)
                {
                    if (page == null)
                        continue;
                    BattleDiceCardModel card=page.card;
                    if (card == null || card==_card)
                        continue;
                    if (card.GetID() == _card.GetID())
                        CopyCount++;
                }
            }
        }
    }
}

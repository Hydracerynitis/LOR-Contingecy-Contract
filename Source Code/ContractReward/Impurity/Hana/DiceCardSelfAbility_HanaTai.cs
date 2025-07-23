using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaTai : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new TaiBuff();
        }
        class TaiBuff: HexagramCardBuf, StartBattleInHandBuf
        {
            public override string keywordIconId => "泰Buf";
            public override string keywordId => "HanaTai";

            public void OnStartBattle_inHand(BattleUnitModel unit)
            {
                unit.bufListDetail.AddBuf(new DamageUP());
                unit.allyCardDetail.DiscardACardByAbility(_card);
            }
            class DamageUP: BattleUnitBuf
            {
                public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnUseCard(card);
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = 35 });
                }
                public override void OnRoundEnd()
                {
                    Destroy();
                }
            }
        }
    }
}

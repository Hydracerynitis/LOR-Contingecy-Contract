using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaMingYi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new MingYiBuff();
        }
        class MingYiBuff: HexagramCardBuf, StartBattleInHandBuf
        {
            public override string keywordIconId => "明夷Buf";
            public override string keywordId => "HanaMingYi";
            public void OnStartBattle_inHand(BattleUnitModel unit)
            {
                List<BattleDiceCardModel> copies= unit.allyCardDetail.GetAllDeck().FindAll(x => x.GetID()==_card.GetID());
                copies.ForEach(x => DeepCopyUtil.EnhanceCard(x, 1, 1));
                unit.allyCardDetail.DiscardACardByAbility(_card);
            }
        }
    }
}

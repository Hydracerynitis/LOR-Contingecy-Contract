using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Vigilant2 : DiceCardSelfAbilityBase
    {
        public override void OnRoundEnd_inHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnRoundEnd_inHand(unit, self);
            unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness,1,unit);
        }
    }
}

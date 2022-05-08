using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Vigilant1 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard() => owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness,2,owner);
        public override void OnRoundEnd_inHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnRoundEnd_inHand(unit, self);
            unit.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}

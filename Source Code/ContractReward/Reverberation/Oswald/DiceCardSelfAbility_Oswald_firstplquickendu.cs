using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_firstplquickendu : DiceCardSelfAbilityBase
    {
        private int count = 0;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            count++;
        }
        public override void OnLoseParrying()
        {
            if(count<3)
                return;
            BattleUnitModel target = this.card?.target;
            if (target == null)
                return;
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, this.owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 2, this.owner);
        }
    }
}

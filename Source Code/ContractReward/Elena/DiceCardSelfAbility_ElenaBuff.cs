using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_ElenaBuff: DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            this.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2);
            this.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2);
        }
        public override bool IsTargetableAllUnit() =>true;
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return targetUnit!=unit;
        }
    }
}

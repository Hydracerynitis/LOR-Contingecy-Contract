using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_Area4Burn : DiceCardAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 4,owner);
        }
    }
}

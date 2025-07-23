using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaCostdown : DiceCardSelfAbilityBase
    {
        public override void OnRoundEnd(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (unit.bufListDetail.HasBuf<BattleUnitBuf_hanaBufCommon>())
                self.AddBuf(new DiceCardSelfAbility_smallBirdEgo.BattleDiceCardBuf_smallBirdCostDown());
        }
    }
}

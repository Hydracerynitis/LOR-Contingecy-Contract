using BaseMod;
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
    public class DiceCardAbility_GamDraw : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana3>())
            {
                owner.allyCardDetail.DrawCards(2);
            }
        }
    }
}

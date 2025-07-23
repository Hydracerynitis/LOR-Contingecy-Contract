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
    public class DiceCardAbility_GeonReuse : DiceCardAbilityBase
    {
        private bool HasReuse = false;
        public override void AfterAction()
        {
            base.AfterAction();
            if(owner.bufListDetail.HasBuf<BattleUnitBuf_hana1>() && !HasReuse)
            {
                ActivateBonusAttackDice();
                HasReuse = true;
            }
        }
    }
}

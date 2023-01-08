using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1990001 : PassiveAbilityBase
    {
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                unit.allyCardDetail.GetHand().ForEach(x => x.exhaust=true);
            };
        }
    }
}

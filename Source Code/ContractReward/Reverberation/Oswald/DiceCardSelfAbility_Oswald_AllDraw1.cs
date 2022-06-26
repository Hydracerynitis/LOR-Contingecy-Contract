using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_AllDraw1 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                unit.allyCardDetail.DrawCards(1);
        }
    }
}

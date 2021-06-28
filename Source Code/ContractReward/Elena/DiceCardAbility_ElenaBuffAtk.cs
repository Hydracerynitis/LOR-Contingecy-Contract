using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_ElenaBuffAtk : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength,2,this.owner);
                unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 2,owner);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1870004: PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            aliveList.Remove(this.owner);
            int count = aliveList.Count;
            if (aliveList.Count <= 0)
                return;
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, count);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, count);
        }
    }
}

using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1930001 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 2));
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 2));
        }
        public override void OnDie()
        {
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 5));
        }
    }
}

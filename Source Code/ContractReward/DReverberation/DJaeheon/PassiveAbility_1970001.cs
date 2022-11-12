using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1970001 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, RandomUtil.Range(1, 3)));
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddBuf(new BattleUnitBuf_Jaeheon_Alive()));
        }
        public override void OnDie()
        {
            base.OnDie();
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.AddBuf(new BattleUnitBuf_Jaeheon_Killed()));
        }
    }
}

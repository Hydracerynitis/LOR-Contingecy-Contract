using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1940001 : PassiveAbilityBase
    {
        private static readonly KeywordBuf[] status = new KeywordBuf[] { KeywordBuf.Strength, KeywordBuf.Endurance, KeywordBuf.Quickness };
        public override void OnRoundStartAfter()
        {
            for(int i=0; i<owner.cardSlotDetail.PlayPoint; i++)
            {
                List<BattleUnitModel> lucky = BattleObjectManager.instance.GetAliveList_random(owner.faction, 1);
                if(lucky.Count > 0)
                    lucky[0].bufListDetail.AddKeywordBufThisRoundByEtc(RandomUtil.SelectOne(status), 1);
            }
        }
        public override void OnDie()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                unit.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 3);
                unit.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 3);
                unit.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 3);
            }
        }
    }
}

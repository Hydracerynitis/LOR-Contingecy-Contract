using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890002 : PassiveAbilityBase
    {
        private BehaviourDetail immuneDetail;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            immuneDetail = RandomUtil.SelectOne<BehaviourDetail>(BehaviourDetail.Slash, BehaviourDetail.Penetrate, BehaviourDetail.Hit);
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == immuneDetail)
                return AtkResist.Immune;
            return base.GetResistHP(origin, detail);
        }
    }
}

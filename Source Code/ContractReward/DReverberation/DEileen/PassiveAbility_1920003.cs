using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1920003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.bufListDetail.AddBuf(new SmokeShield());
        }
        public class SmokeShield: BattleUnitBuf
        {
            public override int GetDamageReductionRate()
            {
                return 5*_owner.bufListDetail.GetKewordBufStack(KeywordBuf.Smoke);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1850003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(18500008));
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(18500009));
        }
    }
}

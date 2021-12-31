using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1850002 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(18500007));
        }

    }
}

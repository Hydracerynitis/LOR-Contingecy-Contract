using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880004: PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(18800008));
        }
    }
}

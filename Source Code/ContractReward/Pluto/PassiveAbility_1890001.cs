using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890001 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.personalEgoDetail.AddCard(18900011);
            this.owner.personalEgoDetail.AddCard(18900012);
            this.owner.personalEgoDetail.AddCard(18900013);
            this.owner.personalEgoDetail.AddCard(18900014);
            this.owner.personalEgoDetail.AddCard(18900015);
        }
    }
}

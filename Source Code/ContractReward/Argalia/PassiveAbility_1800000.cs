using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1800000 : PassiveAbilityBase
    {
        public PassiveAbility_1800000()
        {
        }
        public PassiveAbility_1800000(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(1800000);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(1800000);
            this.rare = Rarity.Unique;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            while (this.owner.allyCardDetail.GetHand().Count < 7)
                this.owner.allyCardDetail.DrawCards(1);
        }
    }
}

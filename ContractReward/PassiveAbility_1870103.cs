using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1870103: PassiveAbilityBase
    {
        public PassiveAbility_1870103(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(1870103);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(1870103);
            this.rare = Rarity.Rare;
        }
    }
}

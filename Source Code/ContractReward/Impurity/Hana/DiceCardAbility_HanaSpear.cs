using BaseMod;
using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_HanaSpear : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            card.target?.TakeDamage(5, DamageType.Card_Ability, this.owner);
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana1>())
                card.target?.TakeDamage(behavior.DiceResultValue, DamageType.Card_Ability, this.owner);
        }
    }
}

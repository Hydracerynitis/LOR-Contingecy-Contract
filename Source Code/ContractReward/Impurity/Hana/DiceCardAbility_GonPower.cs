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
    public class DiceCardAbility_GonPower : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 3 });
            }
        }
    }
}

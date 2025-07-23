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
    public class DiceCardAbility_RiDmg : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = owner.allyCardDetail.GetHand().Count * 2 });
        }
    }
}

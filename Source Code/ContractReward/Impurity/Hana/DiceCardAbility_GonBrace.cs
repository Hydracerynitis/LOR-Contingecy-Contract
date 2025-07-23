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
    public class DiceCardAbility_GonBrace : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
            {
                card.target.currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmg = -5 });
            }
            else
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -5 });
        }
    }
}

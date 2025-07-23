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
    public class DiceCardSelfAbility_HanaSkull : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.cardSlotDetail.RecoverPlayPoint(1);
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana1>())
            {
                card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { max = 3 });
            }
        }
    }
}

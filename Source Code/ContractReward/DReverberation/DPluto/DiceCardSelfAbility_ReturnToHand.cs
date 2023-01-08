using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_ReturnToHand : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (HasUnjust(card.target))
            {
                owner.cardSlotDetail.RecoverPlayPoint(1);
                owner.allyCardDetail._cardInReserved.Remove(card.card);
                owner.allyCardDetail._cardInUse.Remove(card.card);
                owner.allyCardDetail._cardInHand.Add(card.card);
            }
            
        }
        private bool HasUnjust(BattleUnitModel unit)
        {
            return unit.bufListDetail.HasBuf<UnjustPower>() || unit.bufListDetail.HasBuf<UnjustProtection>() || unit.bufListDetail.HasBuf<UnjustSwift>();
        }
    }
}

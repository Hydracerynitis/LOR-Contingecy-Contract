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
    public class DiceCardSelfAbility_PerfectResonance : DiceCardSelfAbilityBase
    {
        public override void OnActivateResonance()
        {
            base.OnActivateResonance();
            this.owner.allyCardDetail.DrawCards(2);
            this.owner.cardSlotDetail.RecoverPlayPoint(3);
            PassiveAbility_1800002 passive = this.owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1800002) as PassiveAbility_1800002;
            if (passive == null)
                return;
            passive.count += 1;
            if (passive.count == 5)
            {
                passive.Upgrade();
                passive.count = 0;
            }
        }
    }
}

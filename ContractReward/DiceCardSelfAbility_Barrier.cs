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
    public class DiceCardSelfAbility_Barrier : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (this.card.target != null)
            {
                BattleUnitBuf unitBufPlutoBarrier = new BattleUnitBuf_Barrier() { Pluto = this.card.owner };
                this.card.target.bufListDetail.AddBuf(unitBufPlutoBarrier);
            }
            this.card.card.exhaust = true;
        }
    }
}

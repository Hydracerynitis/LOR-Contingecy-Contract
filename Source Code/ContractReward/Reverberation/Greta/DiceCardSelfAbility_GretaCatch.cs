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
    public class DiceCardSelfAbility_GretaCatch : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            BattleUnitModel target = this.card.target;
            target.currentDiceAction.DestroyDice(DiceMatch.AllDice);
            target.cardSlotDetail.DestroyCardAll();
            target.bufListDetail.AddBuf(new BattleUnitBuf_FreshMeat());
        }
    }
}

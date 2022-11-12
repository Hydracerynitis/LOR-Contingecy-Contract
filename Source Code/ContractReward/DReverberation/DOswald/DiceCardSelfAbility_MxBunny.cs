using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_MxBunny : DiceCardSelfAbilityBase
    {
        public override void OnEndBattle()
        {
            if (owner.Book.ClassInfo.id == Tools.MakeLorId(19510000))
                return;
            BattleUnitModel bunny = BattleObjectManager.instance.GetAliveList(owner.faction).Find(x => x.Book.ClassInfo.id == Tools.MakeLorId(19510000));       
            if (bunny != null)
            {
                card.owner = bunny;
                StageController.Instance.AddAllCardListInBattle(card, card.target);
            }
        }
    }
}

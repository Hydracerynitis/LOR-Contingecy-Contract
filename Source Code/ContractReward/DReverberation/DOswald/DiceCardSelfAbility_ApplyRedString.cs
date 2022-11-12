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
    public class DiceCardSelfAbility_ApplyRedString : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            if(!card.target.bufListDetail.HasBuf<BattleUnitBuf_RedString>())
                card.target.bufListDetail.AddReadyBuf(new BattleUnitBuf_RedString());
        }
    }
}

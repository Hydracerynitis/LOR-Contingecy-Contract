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
    public class DiceCardSelfAbility_SelfBurn2 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[]{"Burn_Keyword"};
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 2);
        }
    }
}

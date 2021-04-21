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
    public class DiceCardSelfAbility_BurnPowerUp : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[]{"Burn_Keyword"};
        public override void OnUseCard()
        {
            if(card.target==null ||card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn)==null|| card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn).stack<10)
                return;
            this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
        }
    }
}

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
    public class DiceCardSelfAbility_HanaBuf : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana3>())
            {
                card.card.SetCurrentCost(2);
            }
        }
        public override void OnUseCard() => this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
    }
}


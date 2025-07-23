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
    public class DiceCardSelfAbility_HanaFriend : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.card.owner.faction))
            {
                alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, this.owner);
                if(owner.bufListDetail.HasBuf<BattleUnitBuf_hana1>())
                    alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
            }
        }
    }
}

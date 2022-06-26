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
    public class DiceCardSelfAbility_LoseBurn : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[]{"Burn_Keyword"};
        public override void OnStartBattle()
        {
            if (this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) == null)
                return;
            BattleUnitBuf burn = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Burn);
            if (burn.stack >= 5)
            {
                burn.stack -= 5;
                if (burn.stack <= 0)
                    burn.Destroy();
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
                {
                    if (unit != owner)
                        unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 2, owner);
                }
            }
            
            
        }
    }
}

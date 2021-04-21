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
    public class DiceCardSelfAbility_PuppetEndure : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (FindAngelica() != null)
                FindAngelica().bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance,1);
        }
        private BattleUnitModel FindAngelica() => BattleObjectManager.instance.GetAliveList(this.owner.faction).Find((Predicate<BattleUnitModel>)(x => x.Book.GetBookClassInfoId() == 18710000));
    }
}

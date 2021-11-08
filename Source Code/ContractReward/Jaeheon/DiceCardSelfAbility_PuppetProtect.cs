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
    public class DiceCardSelfAbility_PuppetProtect : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (FindAngelica() != null)
            {
                FindAngelica().bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2);
                FindAngelica().bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 2);
            }
        }
        private BattleUnitModel FindAngelica() => BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.Book.GetBookClassInfoId() == Tools.MakeLorId(18710000));
    }
}

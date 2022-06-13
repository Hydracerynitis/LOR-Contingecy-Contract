using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Agony : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(3);
            BattleObjectManager.instance.GetAliveList_random(owner.faction == Faction.Player ? Faction.Enemy : Faction.Player, 2).ForEach(x => x.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 1, owner));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_firstplquickendu : DiceCardSelfAbilityBase
    {
        private int loseCount=0;
        public override void OnLoseParrying()
        {
            ++this.loseCount;
            if (this.loseCount < 3)
                return;
            BattleUnitModel target = this.card?.target;
            if (target == null)
                return;
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, this.owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, this.owner);
            if (owner.passiveDetail.HasPassive<PassiveAbility_1850003>())
            {
                BattleUnitModel ally = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction));
                if (ally == null)
                    return;
                ally.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, this.owner);
                ally.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, this.owner);
            }
        }
    }
}

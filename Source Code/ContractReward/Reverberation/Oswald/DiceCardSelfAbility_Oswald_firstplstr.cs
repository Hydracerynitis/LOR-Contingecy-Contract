using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_firstplstr : DiceCardSelfAbilityBase
    {
        private int loseCount;

        public override void OnLoseParrying()
        {
            if (this.loseCount != 0)
                return;
            ++this.loseCount;
            this.card?.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
            if (owner.passiveDetail.HasPassive<PassiveAbility_1850003>())
            {
                List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
                for (int i=0; i<2 && ally.Count>0; i++)
                {
                    BattleUnitModel unit = RandomUtil.SelectOne(ally);
                    ally.Remove(unit);
                    if (ally == null)
                        continue;
                    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
                }
            }
        }
    }
}

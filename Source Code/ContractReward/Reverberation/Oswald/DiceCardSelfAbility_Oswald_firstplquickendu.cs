using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_firstplquickendu : DiceCardSelfAbilityBase
    {
        private int count = 0;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            count++;
        }
        public override void OnLoseParrying()
        {
            if(count<3)
                return;
            BattleUnitModel target = this.card?.target;
            if (target == null)
                return;
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, this.owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, this.owner);
            if (owner.passiveDetail.HasPassive<PassiveAbility_1850003>())
            {
                List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
                for (int i = 0; i < 2 && ally.Count > 0; i++)
                {
                    BattleUnitModel unit = RandomUtil.SelectOne(ally);
                    ally.Remove(unit);
                    if (ally == null)
                        continue;
                    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, this.owner);
                    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, this.owner);
                }
            }
        }
    }
}

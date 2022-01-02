using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_allpl : DiceCardSelfAbilityBase
    {
        private int loseCount;

        public override void OnLoseParrying() => this.AddLoseCount();

        private void AddLoseCount()
        {
            ++this.loseCount;
            int loseCount = this.loseCount;
            int? count = this.card?.GetOriginalDiceBehaviorList()?.Count;
            if (loseCount >= count.GetValueOrDefault() & count.HasValue)
            {
                this.loseCount = 0;
                this.card?.target?.cardSlotDetail.RecoverPlayPointByCard(2);
                if (owner.passiveDetail.HasPassive<PassiveAbility_1850003>())
                {
                    List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
                    for (int i = 0; i < 2 && ally.Count > 0; i++)
                    {
                        BattleUnitModel unit = RandomUtil.SelectOne(ally);
                        ally.Remove(unit);
                        if (ally == null)
                            continue;
                        unit.cardSlotDetail.RecoverPlayPointByCard(2);
                    }
                }
            }

        }
    }
}

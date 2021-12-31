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
                    BattleUnitModel ally = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction));
                    if (ally == null)
                        return;
                    ally.cardSlotDetail.RecoverPlayPointByCard(2);
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1840004 : PassiveAbilityBase
    {
        private bool _thisRoundActivated;
        public override void OnRoundStart() => this._thisRoundActivated = false;
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || behavior.Detail != BehaviourDetail.Hit || BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count<=0)
                return;
            target.TakeBreakDamage(RandomUtil.Range(1,2), DamageType.Passive, this.owner);
            if (!_thisRoundActivated)
            {
                this._thisRoundActivated = true;
                RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction)).bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this.owner);
            }

        }
    }
}

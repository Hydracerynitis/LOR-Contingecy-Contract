using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880101: PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.owner.SetHp((int)(this.owner.hp-0.1 * owner.MaxHp));
            this.owner.view.Damaged((int)(0.1 * owner.MaxHp), BehaviourDetail.None, (int)(0.1 * owner.MaxHp), this.owner);
            if (this.owner.hp <= 0 && !this.owner.IsDead())
                this.owner.Die();
            else
            {
                owner.allyCardDetail.DrawCards(1);
                owner.cardSlotDetail.RecoverPlayPoint(1);
            }
        }
    }
}

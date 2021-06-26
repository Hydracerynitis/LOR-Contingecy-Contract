using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1870002: PassiveAbilityBase
    {
        public PassiveAbility_1870002()
        {
        }
        public PassiveAbility_1870002(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(1870002);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(1870002);
            this.rare = Rarity.Unique;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if(behavior.card.speedDiceResultValue>=5)
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    breakDmg=1
                });
            if (behavior.Detail != BehaviourDetail.Hit)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 1
            });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.Detail != BehaviourDetail.Hit)
                return;
            behavior.card.target.TakeBreakDamage(2,DamageType.Attack);
        }
    }
}

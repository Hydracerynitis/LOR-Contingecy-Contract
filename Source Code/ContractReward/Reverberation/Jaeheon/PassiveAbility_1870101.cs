using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1870101: PassiveAbilityBase
    {
        public PassiveAbility_1870101(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1870101));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1870101));
            this.rare = Rarity.Rare;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.card.speedDiceResultValue >= 5)
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    breakDmg = 1
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

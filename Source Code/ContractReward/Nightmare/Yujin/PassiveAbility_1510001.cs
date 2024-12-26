using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510001 : PassiveAbilityBase
    {
        public bool onHit = false;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (!onHit)
            {
                Focus.AddStack(owner, 3);
            }
            onHit = false;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            onHit = true;
            return base.BeforeTakeDamage(attacker, dmg);
        }
    }
}

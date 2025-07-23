using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510101 : PassiveAbilityBase
    {
        public bool onHit = false;
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (!onHit)
            {
                Focus.AddStack(owner, 3);
            }
            onHit = false;
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            onHit = true;
        }
    }
}

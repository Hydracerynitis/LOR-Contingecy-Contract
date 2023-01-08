using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1910002 : PassiveAbilityBase
    {
        private bool active;
        private Battle.CreatureEffect.CreatureEffect _effect;
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            if(active)
                owner.battleCardResultLog?.SetCreatureEffectSound("Battle/Philip_Hit");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            active = false;
            _effect?.ManualDestroy();
            _effect = null;
            int burn = 0;
            BattleObjectManager.instance.GetAliveList().ForEach(x => burn += x.bufListDetail.GetKewordBufStack(KeywordBuf.Burn));
            if (burn >= 30)
            {
                active = true;
                _effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("Philip/Philip_Aura_Body", 1f, owner.view, owner.view);
            }         
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (active)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1, dmgRate = 25, breakRate = 25 });
        }
    }
}

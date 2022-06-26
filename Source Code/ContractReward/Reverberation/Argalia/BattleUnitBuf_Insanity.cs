using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_Insanity: BattleUnitBuf
    {
        private int accumulated;
        private int gauge => (int)(this._owner.MaxHp * 0.02);
        public override string keywordId => "Insanity";
        public override string keywordIconId => "Orchestra_Enthusiastic";
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            stack = 0;
            accumulated = 0;
        }
        public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            base.BeforeTakeDamage(attacker, dmg);
            accumulated += dmg;
            for (; accumulated > gauge;)
            {
                accumulated -= gauge;
                stack += 1;
            }
        }
        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            base.OnSuccessAttack(behavior);
            if (behavior.card.card.GetID() == Tools.MakeLorId(18000012))
                return;
            accumulated += behavior.DiceResultDamage;
            for (; accumulated > gauge;)
            {
                accumulated -= gauge;
                stack += 1;
            }
        }
    }
}

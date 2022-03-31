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
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            accumulated += this._owner.history.damageAtOneRound;
            for (; accumulated > gauge;)
            {
                accumulated -= gauge;
                stack += 1;
            }
        }
    }
}

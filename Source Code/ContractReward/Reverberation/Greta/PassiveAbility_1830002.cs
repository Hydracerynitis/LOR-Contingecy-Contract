using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1830002 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3, this.owner);
        }
        public override bool OnBreakGageZero()
        {
            this.owner.LoseHp(50);
            this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
            this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this.owner);
            return true;
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            BattleUnitBuf activatedBuf = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding);
            return activatedBuf != null && activatedBuf.stack > 0 ? base.ChangeDamage(attacker, dmg) + 3.0 : base.ChangeDamage(attacker, dmg);
        }
    }
}

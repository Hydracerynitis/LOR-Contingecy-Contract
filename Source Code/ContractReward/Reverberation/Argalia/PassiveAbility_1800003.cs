using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1800003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Insanity());
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18000011));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18000012));
        }
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(behavior, target);
            if (owner.bufListDetail.FindBuf<BattleUnitBuf_Insanity>() is BattleUnitBuf_Insanity insane)
                insane.OnSuccessAttack(behavior);
        }
    }
}

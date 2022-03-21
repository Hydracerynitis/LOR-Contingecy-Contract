using System;
using System.Collections.Generic;
using UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace Fix
{
    public class PassiveAbility_170003_New : PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            name = PassiveDescXmlList.Instance.GetName(Tools.MakeLorId(2));
            desc = PassiveDescXmlList.Instance.GetDesc(Tools.MakeLorId(2));
            rare = Rarity.Unique;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            this.owner.breakDetail.RecoverBreak(10);
            this.owner?.battleCardResultLog?.SetPassiveAbility(this);
        }
    }
}

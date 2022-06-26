using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1850011 : PassiveAbilityBase
    {
        private bool _bDead;
        private const int _OSWALD_ID = 1305011;
        private bool _bGiveBreakDmgToOswald;

        public override void OnDie() => this._bDead = true;

        public override void OnRoundEndTheLast_ignoreDead()
        {
            if (!this._bDead || this._bGiveBreakDmgToOswald)
                return;
            BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList().Find(x => x.passiveDetail.HasPassive<PassiveAbility_1850003>());
            if (battleUnitModel == null)
                return;
            battleUnitModel.TakeBreakDamage(30, DamageType.Passive);
            this._bGiveBreakDmgToOswald = true;
        }
    }
}

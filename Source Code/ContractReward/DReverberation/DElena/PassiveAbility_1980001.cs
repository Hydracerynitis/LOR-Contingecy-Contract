using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1980001 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                unit.bufListDetail.AddBuf(new BloodFeast());

        }
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                unit.bufListDetail.RemoveBufAll(typeof(BloodFeast));
                unit.LoseHp(unit.MaxHp / 2);
            };
        }
        class BloodFeast: BattleUnitBuf
        {
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                _owner.RecoverHP(2);
            }
        }
    }
}

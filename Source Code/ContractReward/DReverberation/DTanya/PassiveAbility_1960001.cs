using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1960001 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                int Damage = (int)(RandomUtil.Range(3, 6) * 0.01 * unit.MaxHp);
                unit.TakeDamage(Damage);
            }
        }
        public override void OnDie()
        {
            base.OnDie();
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.AddBuf(new BattleUnitBuf_Tanya_Killed()));
        }
    }
}

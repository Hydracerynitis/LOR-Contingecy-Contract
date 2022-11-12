using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1960003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.bufListDetail.AddBuf(new Clown());
        }
        public class Clown: BattleUnitBuf
        {
            public override int GetBreakDamageReductionRate()
            {
                return BattleObjectManager.instance.GetAliveList(_owner.faction).Exists(x => x.Book.ClassInfo.id == Tools.MakeLorId(19500000)) ? 30: 0;
            }
        }
    }
}

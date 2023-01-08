using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1990011 : PassiveAbilityBase
    {
        public override void OnRoundEndTheLast()
        {
            if (BattleObjectManager.instance.GetAliveList(this.owner.faction).Exists(x => x.Book.GetBookClassInfoId()==Tools.MakeLorId(19900000)))
                return;
            this.owner.Die();
        }
    }
}

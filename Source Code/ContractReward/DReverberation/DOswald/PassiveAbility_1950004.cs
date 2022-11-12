using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1950004 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            if (BattleObjectManager.instance.GetAliveList(owner.faction).Exists(x => x.Book.ClassInfo.id == Tools.MakeLorId(19700000)))
                owner.personalEgoDetail.AddCard(Tools.MakeLorId(19500102));
            else
                owner.personalEgoDetail.RemoveCard(Tools.MakeLorId(19500102));
        }
    }
}

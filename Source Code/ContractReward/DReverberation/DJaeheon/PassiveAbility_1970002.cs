using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1970002 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (BattleObjectManager.instance.GetAliveList(owner.faction).Exists(x => x.Book.ClassInfo.id == Tools.MakeLorId(19600000)))
                behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1);
        }
    }
}

using BaseMod;
using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_GonBreak : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.breakDetail.RecoverBreak(5);
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
            {
                BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x =>
                {
                    if (x != owner)
                        x.breakDetail.RecoverBreak(5);
                });
            }
        }
    }
}

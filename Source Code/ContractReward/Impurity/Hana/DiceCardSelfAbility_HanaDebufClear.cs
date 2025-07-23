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
    public class DiceCardSelfAbility_HanaDebufClear : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            int count = 0;
            foreach (BattleUnitBuf activatedBuf in this.owner.bufListDetail.GetActivatedBufList())
            {
                if (activatedBuf.positiveType == BufPositiveType.Negative)
                {
                    if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana3>())
                        count += 1;
                    activatedBuf.Destroy();
                }
            }
            if(count>0)
            {
                owner.bufListDetail.AddBuf(new LightNextScene(count));
            }
        }
        class LightNextScene: BattleUnitBuf
        {
            int count = 0;
            public LightNextScene(int count)
            {
                this.count = count;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.cardSlotDetail.RecoverPlayPoint(count);
                Destroy();
            }
        }
    }
}


using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1940002 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.bufListDetail.AddBuf(new LowerCost());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if(!owner.IsBreakLifeZero())
                owner.cardSlotDetail._playPoint += 1;
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            owner.cardSlotDetail.LosePlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
        }
        class LowerCost: BattleUnitBuf
        {
            public override int GetCardCostAdder(BattleDiceCardModel card)
            {
                return -2;
            }
        }
    }
}

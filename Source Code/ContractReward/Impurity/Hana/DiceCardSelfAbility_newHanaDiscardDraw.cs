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
    public class DiceCardSelfAbility_newHanaDiscardDraw : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            this.owner.allyCardDetail.DiscardACardRandomlyByAbility(1);
            this.owner.bufListDetail.AddBuf(new DiceCardSelfAbility_hanaDiscardDraw.BattleUnitBuf_nextDraw2());
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana4>())
            {
                owner.bufListDetail.AddReadyBuf(new IncreasedHand());
            }

        }
        class IncreasedHand: BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                _owner.allyCardDetail.SetMaxDrawHand(10);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                _owner.allyCardDetail.SetMaxDrawHand(8);
                Destroy();
            }
        }
    }
}


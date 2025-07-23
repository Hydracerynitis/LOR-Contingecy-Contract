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
    public class DiceCardSelfAbility_HanaUltimate : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[2]
          {
            "newHanaUltimate1",
            "newHanaUltimate2"
          };
        public override void OnUseCard()
        {
            int handCount = owner.allyCardDetail.GetHand().Count;
            if(handCount > 4 )
            {
                BattleUnitBuf battleUnitBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_hanaBufCommon);
                if (battleUnitBuf == null)
                    return;
                if (battleUnitBuf is BattleUnitBuf_hana1)
                {
                    card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new Recycle()));
                }
                if (battleUnitBuf is BattleUnitBuf_hana2)
                {
                    owner.RecoverHP((int)(0.3 * owner.MaxHp));
                    owner.breakDetail.RecoverBreak(owner.breakDetail.GetDefaultBreakGauge());
                }
                if (battleUnitBuf is BattleUnitBuf_hana3)
                    BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x =>
                    {
                        x.cardSlotDetail.RecoverPlayPoint(2);
                    });
                if (battleUnitBuf is BattleUnitBuf_hana4)
                {
                    owner.bufListDetail.AddBuf(new nextDraw3());
                }
            }
            this.owner.allyCardDetail.DiscardACardByAbility(owner.allyCardDetail.GetHand());
            this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = 2
            });
        }
        public class Recycle: DiceCardAbilityBase
        {
            private bool HasRecycle=false;
            public override void OnSucceedAttack()
            {
                if (HasRecycle)
                    return;
                this.ActivateBonusAttackDice();
                HasRecycle = true;
            }
        }
        public class nextDraw3 : BattleUnitBuf
        {
            public override void OnRoundEndTheLast()
            {
                this._owner.allyCardDetail.DrawCards(3);
                this.Destroy();
            }
        }
    }
}

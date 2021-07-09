using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1860001 : PassiveAbilityBase
    {
        public override int SpeedDiceNumAdder()
        {
            int num1 = 0;
            int num2 = this.owner.emotionDetail.SpeedDiceNumAdder();
            if (num2 > 0)
                num1 = -num2;
            return num1;
        }
        public override void OnEndParrying()
        {
            base.OnEndParrying();
            if(this.owner.cardSlotDetail.keepCard.cardBehaviorQueue.Count>0)
                this.owner.cardSlotDetail.keepCard.Reset();
        }
        public BattlePlayingCardDataInUnitModel Retaliate(BattlePlayingCardDataInUnitModel attackerCard)
        {
            if (owner.IsBreakLifeZero())
                return null;
            List<BattleDiceCardModel> hand = this.owner.allyCardDetail.GetHand().FindAll(x => x.GetCost() <= this.owner.cardSlotDetail.PlayPoint - this.owner.cardSlotDetail.ReservedPlayPoint && CheckRange(x.XmlData.Spec.Ranged));
            if (hand.Count <= 0)
                return null;
            BattleDiceCardModel card = hand[0];
            owner.allyCardDetail.UseCard(card);
            owner.allyCardDetail.SpendCard(card);
            owner.cardSlotDetail.ReserveCost(card.GetCost());
            owner.cardSlotDetail.SpendCost(card.GetCost());
            BattlePlayingCardDataInUnitModel retaliate = new BattlePlayingCardDataInUnitModel()
            {
                owner = this.owner,
                card = card,
                cardAbility = card.CreateDiceCardSelfAbilityScript(),
                target = attackerCard.owner,
                slotOrder = attackerCard.targetSlotOrder,
                targetSlotOrder = attackerCard.slotOrder
            };
            if (retaliate.cardAbility != null)
                retaliate.cardAbility.card = retaliate;
            retaliate.ResetCardQueue();
            return retaliate;
        }
        private bool CheckRange(CardRange range)
        {
            return range != CardRange.FarArea && range != CardRange.FarAreaEach && range != CardRange.Instance;
        }
    }
}

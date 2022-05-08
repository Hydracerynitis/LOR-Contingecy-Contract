using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Contingecy_Contract;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1860001 : PassiveAbilityBase, Retaliater
    {
        public override int SpeedDiceNumAdder()
        {
            int num1 = 0;
            int num2 = owner.emotionDetail.SpeedDiceNumAdder();
            if (num2 > 0)
                num1 = -num2;
            return num1;
        }
        public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
        {
            if (owner.cardSlotDetail.keepCard.cardBehaviorQueue.Count > 0)
                owner.cardSlotDetail.keepCard.Reset();
        }
        public BattlePlayingCardDataInUnitModel Retaliate(BattlePlayingCardDataInUnitModel attackerCard)
        {
            if (owner.IsBreakLifeZero())
                return null;
            List<BattleDiceCardModel> hand = owner.allyCardDetail.GetHand().FindAll(x => x.GetCost() <= owner.cardSlotDetail.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint && CheckRange(x.XmlData.Spec.Ranged) && x.XmlData.Spec.affection!=CardAffection.TeamNear);
            if (hand.Count <= 0)
                return null;
            BattleDiceCardModel card = hand[hand.Count-1];
            owner.allyCardDetail.UseCard(card);
            owner.allyCardDetail.SpendCard(card);
            owner.cardSlotDetail.ReserveCost(card.GetCost());
            owner.cardSlotDetail.SpendCost(card.GetCost());
            BattlePlayingCardDataInUnitModel retaliate = new BattlePlayingCardDataInUnitModel()
            {
                owner = owner,
                card = card,
                cardAbility = card.CreateDiceCardSelfAbilityScript(),
                target = attackerCard.owner,
                slotOrder = attackerCard.targetSlotOrder,
                targetSlotOrder = attackerCard.slotOrder
            };
            if (retaliate.cardAbility != null)
                retaliate.cardAbility.card = retaliate;
            retaliate.ResetCardQueueWithoutStandby();
            return retaliate;
        }
        private bool CheckRange(CardRange range)
        {
            return range != CardRange.FarArea && range != CardRange.FarAreaEach && range != CardRange.Instance;
        }
    }
}

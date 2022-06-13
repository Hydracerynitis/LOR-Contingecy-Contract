using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandPurple : EmotionCardAbilityBase
    {
        private int count = 0;
        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            int diceMin = behavior.GetDiceMin();
            int diceMax = behavior.GetDiceMax();
            if (diceResult <= diceMin)
            {
                diceResult = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
                behavior.owner.battleCardResultLog?.SetVanillaDiceValue(diceMin);
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (count < 2)
            {
                count++;
                return;
            }
            List<BehaviourDetail> list = new List<BehaviourDetail>();
            int resistValue1 = this.GetResistValue(BehaviourDetail.Slash,curCard);
            int resistValue2 = this.GetResistValue(BehaviourDetail.Penetrate,curCard);
            int resistValue3 = this.GetResistValue(BehaviourDetail.Hit, curCard);
            int num = resistValue1;
            if (resistValue2 > num)
                num = resistValue2;
            if (resistValue3 > num)
                num = resistValue3;
            if (num == resistValue1)
                list.Add(BehaviourDetail.Slash);
            if (num == resistValue2)
                list.Add(BehaviourDetail.Penetrate);
            if (num == resistValue3)
                list.Add(BehaviourDetail.Hit);
            BehaviourDetail behaviourDetail = RandomUtil.SelectOne<BehaviourDetail>(list);
            foreach (BattleDiceBehavior diceBehavior in curCard.GetDiceBehaviorList())
            {
                if (this.IsAttackDice(diceBehavior.behaviourInCard.Detail))
                {
                    diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                    diceBehavior.behaviourInCard.Detail = behaviourDetail;
                }
            }
            count = 0;
        }

        private int GetResistValue(BehaviourDetail detail, BattlePlayingCardDataInUnitModel curCard) => Mathf.RoundToInt((float)((0.0 + (double)BookModel.GetResistRate(curCard.target.Book.GetResistHP(detail)) + (double)BookModel.GetResistRate(curCard.target.Book.GetResistBP(detail))) * 10.0));
    }
}

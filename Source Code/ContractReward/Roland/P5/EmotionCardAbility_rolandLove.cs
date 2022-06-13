using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using System;

namespace ContractReward
{
    public class EmotionCardAbility_rolandLove : EmotionCardAbilityBase
    {
        private Dictionary<BehaviourDetail, int> HitDic = new Dictionary<BehaviourDetail, int>() ;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            HitDic.Add(BehaviourDetail.Slash, 0);
            HitDic.Add(BehaviourDetail.Penetrate, 0);
            HitDic.Add(BehaviourDetail.Hit, 0);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            BehaviourDetail detail = behavior.behaviourInCard.Detail;
            if (HitDic.ContainsKey(detail))
            {
                HitDic[detail] += 1;
                return Math.Min(2, HitDic[detail] * 2);
            }
            return base.GetDamageReduction(behavior);
        }
        public override void OnRoundEnd()
        {
            HitDic[BehaviourDetail.Slash] = 0;
            HitDic[BehaviourDetail.Penetrate] = 0;
            HitDic[BehaviourDetail.Hit] = 0;
            base.OnRoundEnd();
        }
        public override void OnStartOneSideAction(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnStartOneSideAction(curCard);
            curCard.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus() { power = 3 });
        }
    }
}

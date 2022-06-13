using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandLiu : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card == null || behavior.card.card.GetSpec().Ranged != CardRange.Near)
                return;
            behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 1);
            behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BurnSpread, 2);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if(curCard.card.XmlData.Spec.Ranged==CardRange.FarArea || curCard.card.XmlData.Spec.Ranged == CardRange.FarAreaEach)
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 2);
            }
        }
    }
}

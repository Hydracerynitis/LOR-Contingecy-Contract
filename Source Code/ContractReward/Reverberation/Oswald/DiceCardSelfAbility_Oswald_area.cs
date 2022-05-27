using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_area : DiceCardSelfAbility_AoeCoolDown
    {
        public override void OnUseAoe()
        {
            if (this.card == null)
                return;
            List<KeywordBuf> keywordList = new List<KeywordBuf>(){KeywordBuf.Vulnerable, KeywordBuf.Disarm, KeywordBuf.Weak, KeywordBuf.Protection, KeywordBuf.Strength, KeywordBuf.Endurance};
            BattleUnitModel target = this.card.target;
            List<BattlePlayingCardDataInUnitModel.SubTarget> subTargets = this.card.subTargets;
            if (target != null && !target.IsDead())
                target.bufListDetail.AddKeywordBufByCard(this.GetRandomBuf(keywordList), 2, this.owner);
            foreach (BattlePlayingCardDataInUnitModel.SubTarget subTarget in subTargets)
            {
                if (subTarget != null && subTarget.target != null && !subTarget.target.IsDead())
                    subTarget.target.bufListDetail.AddKeywordBufByCard(this.GetRandomBuf(keywordList), 2, this.owner);
            }
        }
        private KeywordBuf GetRandomBuf(List<KeywordBuf> keywordList) => RandomUtil.SelectOne(keywordList);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1810001: PassiveAbilityBase
    {
        public override void OnLevelUpEmotion()
        {
            base.OnLevelUpEmotion();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 3, this.owner);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int num = this.owner.emotionDetail.EmotionLevel / 2;
            if (num <= 0 || behavior == null)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = num
            });
        }
    }
}

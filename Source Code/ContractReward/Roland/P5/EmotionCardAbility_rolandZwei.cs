using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandZwei : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (IsDefenseDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, RandomUtil.Range(1, 2));
        }
    }
}

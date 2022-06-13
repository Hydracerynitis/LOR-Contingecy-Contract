using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandDawn : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior.card.target;
            if (target.bufListDetail.HasBuf<BattleUnitBuf_burn>())
                target.TakeBreakDamage(RandomUtil.Range(1, 2));
            if (behavior.TargetDice != null && behavior.DiceVanillaValue>behavior.TargetDice.DiceVanillaValue+5)
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 6);
        }
    }
}

using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1930002 : PassiveAbilityBase
    {
        Dictionary<BattleUnitModel, int> triggerTarget = new Dictionary<BattleUnitModel, int>();
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            triggerTarget.Clear();
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card.target;
            if (target == null ||target.bufListDetail.GetKewordBufAllStack(KeywordBuf.Bleeding) < 5)
                return;
            if (triggerTarget.TryGetValue(target, out int times) && times >= 3)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this.owner);
            if(triggerTarget.ContainsKey(target))
                triggerTarget.Add(target, 1);
            else
                triggerTarget[target] += 1;
        }
    }
}

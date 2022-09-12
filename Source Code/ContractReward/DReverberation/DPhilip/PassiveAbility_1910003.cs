using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1910003 : PassiveAbilityBase
    {
        private Dictionary<BattleUnitModel, int> WinClash = new Dictionary<BattleUnitModel, int>();
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            WinClash.Clear();
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (behavior.TargetDice == null)
                return;
            BattleUnitModel enemy = behavior.TargetDice.owner;
            if (WinClash.ContainsKey(enemy))
                WinClash[enemy] += 1;
            else
                WinClash.Add(enemy, 1);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            foreach(KeyValuePair<BattleUnitModel,int> kvp in WinClash)
            {
                if (kvp.Value >= 6)
                    kvp.Key.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Arrest, 1);
            }
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitBuf arrest = behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Arrest);
            if(arrest!=null && arrest.stack >= 3)
            {
                behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
                behavior.card.target.bufListDetail.RemoveBufAll(KeywordBuf.Arrest);
            }
        }
    }
}

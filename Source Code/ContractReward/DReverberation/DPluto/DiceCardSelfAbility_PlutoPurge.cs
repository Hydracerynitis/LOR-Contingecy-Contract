using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_PlutoPurge : DiceCardSelfAbility_AoeCoolDown
    {
        private static KeywordBuf[] purgeType = new KeywordBuf[] { KeywordBuf.Strength, KeywordBuf.Endurance, KeywordBuf.Protection, KeywordBuf.BreakProtection };
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            foreach(KeywordBuf type in purgeType)
            {
                BattleUnitBuf buf = target.bufListDetail.GetActivatedBuf(type);
                if (buf != null)
                {
                    target.bufListDetail.RemoveBuf(buf);
                    for (int i = 0; i < buf.stack; i++)
                    {
                        target.TakeDamage(4);
                        target.battleCardResultLog.SetDamageTaken(4, 4, BehaviourDetail.Penetrate);
                    }
                }
            }
        }
    }
}

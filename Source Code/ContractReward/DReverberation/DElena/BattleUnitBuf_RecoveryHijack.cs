using LOR_DiceSystem;
using Contingecy_Contract;
using System.Collections.Generic;
using HP = SummonLiberation.Harmony_Patch;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using System;
using UnityEngine;

namespace ContractReward
{
    public class BattleUnitBuf_RecoveryHijack : BattleUnitBuf, RecoverHpBuf
    {
        private BattleUnitModel user;
        public override string keywordIconId => "ForbidRecovery";
        public override string keywordId => "RecoveryHijack";
        public BattleUnitBuf_RecoveryHijack(BattleUnitModel user)
        {
            this.user = user;
            stack = 2;
        }
        public void OnRecoverHp(int v)
        {
            if (user == null)
                return;
            user.RecoverHP(v / 2);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            stack--;
            if (stack <= 0)
                Destroy();
        }
    }
}

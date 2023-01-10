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
    public class BattleUnitBuf_Disarray: BattleUnitBuf
    {
        public override string keywordIconId => "PowerUpOrchestraEgo";
        public override string keywordId => "Disarray";
        public override bool IsControllable => false;
        public override bool TeamKill()
        {
            return true;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.card.target.faction == _owner.faction)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = 50 });
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --stack;
            if (this.stack > 0)
                return;
            this.Destroy();
        }
    }
}

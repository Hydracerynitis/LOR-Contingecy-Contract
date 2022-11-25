using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1980002 : PassiveAbilityBase
    {
        private RecoverIndicator buf;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            buf = new RecoverIndicator() { stack=0};
            owner.bufListDetail.AddBuf(buf);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = owner.UnitData.historyInWave.healed / 40 });
        }
        public override void OnRecoverHp(int amount)
        {
            base.OnRecoverHp(amount);
            buf.SetStack(amount);
        }
        public override void OnDestroyed()
        {
            base.OnDestroyed();
            owner.bufListDetail.RemoveBuf(buf);
        }
        class RecoverIndicator: BattleUnitBuf
        {
            public override string keywordIconId => "Nosferatu_Blood";
            public override string keywordId => "DElenaIndicator";
            public void SetStack(int v)
            {
                stack = _owner.UnitData.historyInWave.healed + v;
            }
        }
    }
}

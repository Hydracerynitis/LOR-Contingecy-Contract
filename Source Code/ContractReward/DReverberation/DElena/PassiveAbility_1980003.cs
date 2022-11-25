using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1980003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.SetKnockoutInsteadOfDeath(true);
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            base.OnRoundEndTheLast_ignoreDead();
            if (owner.IsDead())
            {
                owner.bufListDetail.AddBuf(new ImmortalProtocal());
                owner.passiveDetail.PassiveList.ForEach(x => x.destroyed = true);
                owner.passiveDetail.AddPassive(new LorId(10001));
                owner.Revive(owner.MaxHp);
                owner.RecoverBreakLife(1);
                owner.breakDetail.ResetBreakDefault();
                owner.breakDetail.nextTurnBreak = false;
                owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                owner._isKnockout = false;
                owner.SetKnockoutInsteadOfDeath(false);
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_knockout));
            }
        }
        class ImmortalProtocal: BattleUnitBuf
        {
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { hpAdder = -60, breakGageAdder = -30 };
            }
        }
    }
}

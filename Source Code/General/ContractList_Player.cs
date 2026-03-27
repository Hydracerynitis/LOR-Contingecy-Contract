using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Damage_L : ContingecyContract
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -20 * Level });
            }
        }
    }
    public class ContingecyContract_BreakDamage_L : ContingecyContract
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = -20 * Level });
            }
        }
    }
    public class ContingecyContract_Power_L : ContingecyContract
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = -Level });
        }
    }
    public class ContingecyContract_Hp_L : ContingecyContract
    {
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate = -20 * Level };
        }
    }
    public class ContingecyContract_Bp_L : ContingecyContract
    {
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { breakRate = -20 * Level };
        }
    }
    public class ContingecyContract_Darkness : ContingecyContract
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (CCInitializer.CombaltData.ContainsKey(self.UnitData))
                return;
            this.owner.cardSlotDetail.LosePlayPoint(this.owner.cardSlotDetail.PlayPoint);
            this.owner.cardSlotDetail.RecoverPlayPoint(4 - Level);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (Singleton<StageController>.Instance.RoundTurn == 1)
                this.owner.cardSlotDetail.SetRecoverPointDefault();
        }
    }
    public class ContingecyContract_NoBuff : ContingecyContract
    {
    }
    public class ContingecyContract_NoEGO: ContingecyContract
    {
    }
    public class ContingecyContract_NoEmotion : ContingecyContract
    {

    }
}

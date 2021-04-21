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
        public ContingecyContract_Damage_L(int level)
        {
            this.Level = level;
        }
        public override ContractType Type => ContractType.Passive;
        public override string[] GetFormatParam => new string[] { (20*Level).ToString()};
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
        public ContingecyContract_BreakDamage_L(int level)
        {
            this.Level = level;
        }
        public override ContractType Type => ContractType.Passive;
        public override string[] GetFormatParam => new string[] { (20 * Level).ToString() };
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = -20 * Level });
            }
        }
    }
    public class ContingecyContract_Hp_L : ContingecyContract
    {
        public ContingecyContract_Hp_L(int level)
        {
            this.Level = level;
        }
        public override ContractType Type => ContractType.Buff;
        public override string[] GetFormatParam => new string[] { (20 * Level).ToString() };
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate = -20 * Level };
        }
    }
    public class ContingecyContract_Bp_L : ContingecyContract
    {
        public ContingecyContract_Bp_L(int level)
        {
            this.Level = level;
        }
        public override ContractType Type => ContractType.Buff;
        public override string[] GetFormatParam => new string[] { (20 * Level).ToString() };
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { breakRate = -20 * Level };
        }
    }
    public class ContingecyContract_Darkness : ContingecyContract
    {
        public ContingecyContract_Darkness(int level)
        {
            this.Level = 6 - level;
        }
        public override ContractType Type => ContractType.Passive;
        public override string[] GetFormatParam => new string[] { (Level).ToString() };
        public override void OnWaveStart()
        {
            this.owner.cardSlotDetail.LosePlayPoint(this.owner.cardSlotDetail.PlayPoint);
            this.owner.cardSlotDetail.RecoverPlayPoint(Level);
            this.owner.cardSlotDetail.SetRecoverPoint(0);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (Singleton<StageController>.Instance.RoundTurn == 1)
                this.owner.cardSlotDetail.SetRecoverPointDefault();
        }
    }
}

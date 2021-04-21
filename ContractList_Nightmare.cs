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
    //死协会
    public class ContingecyContract_Yujin : ContingecyContract
    {
        public ContingecyContract_Yujin(int level)
        {
            this.Level = level - 1;
        }
        public override string[] GetFormatParam => new string[] { (25*Level).ToString(),Level.ToString()};
        public override ContractType Type => ContractType.Special;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.abilityList.Exists((Predicate<DiceCardAbilityBase>)(x => x is DiceCardAbility_yujin)))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = Level });
            }
        }
        public override void OnRoundStart()
        {
            if (Harmony_Patch.CombaltData.ContainsKey(this.owner.UnitData))
                return;
            this.owner.RecoverHP((int)(this.owner.MaxHp / 4 * Level));
            Harmony_Patch.CombaltData.Add(this.owner.UnitData, (int)this.owner.hp);
            base.OnWaveStart();
        }
    }
    public class ContingecyContract_ValTem : ContingecyContract
    {
        public ContingecyContract_ValTem(int level)
        {
            this.Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (20*Level).ToString(),(1+2*Level).ToString(),Level.ToString()};
        private bool IsVal() => this.owner.UnitData.unitData.EnemyUnitId == 40002;
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (IsVal() && behavior.card.GetOriginalDiceBehaviorList().FindAll((Predicate<DiceBehaviour>)(x => x.Type != BehaviourType.Standby)).Count == 1)
            {
                for (int i = 0; i < Level; i++)
                    behavior.TargetDice.card.DestroyDice(DiceMatch.NextDice);
            }
        }
        public override void OnRollSpeedDice()
        {
            if (IsVal())
                return;
            foreach (SpeedDice dice in this.owner.speedDiceResult)
            {
                if (dice.value < 1 + 2 * Level)
                    dice.value = 1 + 2 * Level;
            }
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate = 20 * Level, breakRate = 20 * Level };
        }
    }
}

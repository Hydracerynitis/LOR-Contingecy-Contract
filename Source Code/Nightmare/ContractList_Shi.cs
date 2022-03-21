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
    public class ContingecyContract_Shi_Yujin : ContingecyContract
    {
        public ContingecyContract_Shi_Yujin(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 40001;
        public override string[] GetFormatParam(string language) => new string[] { (25*Level).ToString(),Level.ToString()};
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            BattleDiceCardModel card = self.allyCardDetail.GetAllDeck().Find(x => x.GetID() == 501001);
            DeepCopyUtil.EnhanceCard(card, Level, 0);
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
    public class ContingecyContract_Shi_ValTem : ContingecyContract
    {
        public ContingecyContract_Shi_ValTem(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 40002 || EnemyId ==40003;
        public override string[] GetFormatParam(string language) => new string[] { (20*Level).ToString(),(1+2*Level).ToString(),Level.ToString()};
        private bool IsVal() => this.owner.UnitData.unitData.EnemyUnitId == 40002;
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (IsVal() && behavior.card.GetOriginalDiceBehaviorList().FindAll(x => x.Type != BehaviourType.Standby).Count == 1)
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

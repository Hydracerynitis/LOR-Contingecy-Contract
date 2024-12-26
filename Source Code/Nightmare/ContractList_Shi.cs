using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

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
        public override string[] GetFormatParam(string language) => new string[] { (Level - 1).ToString(), Level.ToString() };
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.allyCardDetail.AddNewCardToDeck(501001);
            foreach(BattleDiceCardModel card in self.allyCardDetail.GetAllDeck().FindAll(x => x.GetID() == 501001))
            {
                DeepCopyUtil.EnhanceCard(card, Level, 0);
                DeepCopyUtil.ChangeCost(card, -Level + 1);
            }
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
    public class ContingecyContract_Shi : ContingecyContract
    {
        public ContingecyContract_Shi(int level)
        {
            this.Level = level;
        }
        public class Enhanced_passive_241301 : PassiveAbilityBase
        {
            private int _stack;
            public override void Init(BattleUnitModel self)
            {
                base.Init(self);
                name = PassiveDescXmlList.Instance.GetName(Tools.MakeLorId(3));
                desc = PassiveDescXmlList.Instance.GetDesc(Tools.MakeLorId(3));
                rare = Rarity.Uncommon;
            }
            public override void OnWaveStart() => this._stack = 0;

            public override void OnRoundStart()
            {
                if (this._stack <= 0)
                    return;
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1*_stack, this.owner);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1 * _stack, this.owner);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3 * _stack, this.owner);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 3 * _stack, this.owner);
            }

            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                if (unit.faction != this.owner.faction || this._stack >= 2)
                    return;
                ++this._stack;
            }
        }
    }
}

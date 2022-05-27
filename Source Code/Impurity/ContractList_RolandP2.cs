using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using BaseMod;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Sound;

namespace Contingecy_Contract
{
    public class ContingecyContract_Roland2nd_Smoke : ContingecyContract
    {
        private int _dmgreduction;
        public ContingecyContract_Roland2nd_Smoke(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (2+Level).ToString(), (2 + 2*Level).ToString(), (Level*50).ToString() };
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60106 || EnemyId == 60006;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.passiveDetail.PassiveList.Find(x => x is PassiveAbility_170102) is PassiveAbility_170102 passive)
            {
                passive.desc = TextDataModel.GetText("Enhance_HeavySmoke_Passive", (2 + Level).ToString(), (2 + 2 * Level).ToString());
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ApplyDiceAbility(DiceMatch.AllDice, new DiceCardAbility_immuneDestory());
        }
        public override int SpeedDiceNumAdder()
        {
            return owner.UnitData.unitData.EnemyUnitId== 60106 ? 1: 0;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (owner.UnitData.unitData.EnemyUnitId == 60106)
                return new StatBonus() {hpRate=50*Level, breakRate=50*Level };
            return base.GetStatBonus(owner);
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card.target;
            if ((target != null ? (target.bufListDetail.GetKewordBufStack(KeywordBuf.HeavySmoke) > 0 ? 1 : 0) : 0) == 0)
                return;
            this.owner.battleCardResultLog?.SetPassiveAbility((PassiveAbilityBase)this);
            behavior.ApplyDiceStatBonus(new DiceStatBonus(){dmg = Level });
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            this._dmgreduction = 0;
            if (attacker != null && attacker.bufListDetail.GetKewordBufStack(KeywordBuf.HeavySmoke) > 0)
                this._dmgreduction = 2* Level;
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override int GetDamageReductionAll() => this._dmgreduction;
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            owner.allyCardDetail.DrawCards(1);
        }
    }
    public class ContingecyContract_Roland2nd_Secret : ContingecyContract
    {
        public ContingecyContract_Roland2nd_Secret(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (25 + Level * 25).ToString() };
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60106 || EnemyId == 60006;
        }
        public override void OnRoundEnd_before()
        {
            base.OnRoundEnd_before();
            if (owner.UnitData.unitData.EnemyUnitId == 60006)
                return;
            owner.lastAttacker = null;
        }
        public override void OnDie()
        {
            if (owner.UnitData.unitData.EnemyUnitId == 60006 || BattleObjectManager.instance.GetAliveList(owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId== 60006) ==null)
                return;
            owner.lastAttacker?.breakDetail.TakeBreakDamage((int)((0.25 + 0.25 * Level) * owner.lastAttacker.breakDetail.GetDefaultBreakGauge()));
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (owner.UnitData.unitData.EnemyUnitId == 60006)
                return new StatBonus() { breakGageAdder = 350 };
            return base.GetStatBonus(owner);
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.UnitData.unitData.EnemyUnitId == 60006)
                return;
            foreach (BattleDiceCardModel card in self.allyCardDetail.GetAllDeck())
            {
                if (card.GetID() == 702107)
                    DeepCopyUtil.EnhanceCard(card, 1, 1);
                if (card.GetID() == 702108)
                    DeepCopyUtil.EnhanceCard(0,card, 0, 4);
                if (card.GetID() == 702109)
                    DeepCopyUtil.EnhanceCard(card, 0,4);
            }
        }
        public override int SpeedDiceNumAdder()
        {
            return owner.UnitData.unitData.EnemyUnitId == 60006 ? 2 : 0;
        }
        public override void OnRoundStartAfter()
        {
            if (owner.UnitData.unitData.EnemyUnitId == 60006)
            {
                owner.allyCardDetail.AddTempCard(RandomUtil.SelectOne(702101, 702102, 702103, 702104));
                owner.allyCardDetail.AddTempCard(RandomUtil.SelectOne(702101, 702102, 702103, 702104));
            }
        }
    }
    public class ContingecyContract_Roland2nd : ContingecyContract
    {
        public ContingecyContract_Roland2nd(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60106 || EnemyId == 60006;
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (origin == AtkResist.Vulnerable)
                return AtkResist.Endure;
            if (origin == AtkResist.Weak)
                return AtkResist.Resist;
            return base.GetResistBP(origin, detail);
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (origin == AtkResist.Vulnerable)
                return AtkResist.Endure;
            if (origin == AtkResist.Weak)
                return AtkResist.Resist;
            return base.GetResistBP(origin, detail);
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if (owner.UnitData.unitData.EnemyUnitId == 60106)
                return;
            foreach (BattleDiceCardModel card in owner.allyCardDetail.GetAllDeck())
            {
                DeepCopyUtil.EnhanceCard(card, 1, 1);
            }
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.UnitData.unitData.EnemyUnitId == 60006)
                return;
            foreach (BattleDiceCardModel card in self.allyCardDetail.GetAllDeck())
            {
                DeepCopyUtil.EnhanceCard(card, 1, 1);
            }
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.HeavySmoke, 3);
        }
    }
}

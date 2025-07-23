using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using BaseMod;
using UI;

namespace Contingecy_Contract
{
    public class ContingecyContract_DElena_Hp : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1408011;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpAdder = 200 };
        }
        public override int GetDamageReductionRate()
        {
            return 25;
        }
    }
    public class ContingecyContract_DElena_Aoe : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1408011;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (owner.hp < owner.MaxHp / 2)
                owner.RecoverHP(2 * behavior.DiceResultDamage);
            else
                owner.RecoverHP( behavior.DiceResultDamage);
        }
    }
    public class ContingecyContract_DPluto_Contract : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1409011;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            BattleDiceCardModel contract = owner.allyCardDetail.GetHand().Find(x => x.GetID().GreaterEqual(707901) && x.GetID().LesserEqual(707903));
            if (contract != null)
            {
                owner.allyCardDetail.ExhaustACard(contract);
                owner.allyCardDetail.AddNewCard(Tools.MakeLorId(contract.XmlData._id % 10 + 1)).SetPriorityAdder(contract.GetPriorityAdder());
            }
        }
    }
    public class ContingecyContract_DPluto_Shade : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId==1409011;
        }
    }
    public class ContingecyContract_DArgalia_Sonata : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1410011;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                unit.emotionDetail.LevelUp();
                unit.emotionDetail.LevelUp();
            }
        }
        public class PassiveAbility_1410013_New : PassiveAbility_1410013
        {
            public override void OnRoundStart()
            {
                if (PassiveAbility_1410014.IsBattleEnd())
                    return;
                ++this._elapsedRound;
                if (this._elapsedRound % 2 != 0)
                    return;
                this._elapsedRound = 0;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
                {
                    BattleUnitModel u = alive;
                    if (this._idList.Exists((Predicate<int>)(x => u.UnitData.unitData.EnemyUnitId == x)))
                        u.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.UpSurge, 1, this.owner);
                }
            }
        }
    }
    public class ContingecyContract_DArgalia_Ternaria : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1410011;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.faction == owner.faction)
                    unit.bufListDetail.AddBuf(new UpSurgeSideGrade());
                unit.bufListDetail.AddBuf(new StatBonusBuf());
            }
        }
        class StatBonusBuf : BattleUnitBuf
        {
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { hpAdder=100, breakGageAdder=50};
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (owner.faction==Faction.Player)
                {
                    owner.SetHp((int)owner.hp + 100);
                    owner.breakDetail.breakGauge += 50;
                }
            }
        }
        class UpSurgeSideGrade : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                int stack = _owner.bufListDetail.GetKewordBufStack(KeywordBuf.UpSurge);
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50 * stack, breakRate = 50 * stack });
            }
        }

    }
}

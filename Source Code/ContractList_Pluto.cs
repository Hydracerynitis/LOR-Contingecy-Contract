using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using HarmonyLib;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Pluto_Shadow : ContingecyContract
    {
        public ContingecyContract_Pluto_Shadow(int level)
        {
            Level = level;
        }
        public override ContractType Type => ContractType.Special;
        private PassiveAbility_1309021 copypassive;
        public override string[] GetFormatParam => new string[] { (35 + 15 * Level).ToString(), Level.ToString() };
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            copypassive = self.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1309021) as PassiveAbility_1309021;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (copypassive != null)
                return new StatBonus() { hpRate = 35 + 15 * Level, breakRate = 35 + 15 * Level };
            else
                return base.GetStatBonus(owner);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (copypassive == null)
                return;
            if(behavior.card.target==typeof(PassiveAbility_1309021).GetField("_copyUnit",AccessTools.all).GetValue(copypassive) as BattleUnitModel)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = Level });
            }
        }
    }
    public class ContingecyContract_Pluto_Barrier : ContingecyContract
    {
        public ContingecyContract_Pluto_Barrier(int level)
        {
            Level = level;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (5*Level).ToString(), (20 + level * 10).ToString() };
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate=100};
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
            {
                if(unit.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Pluto_Barrier) != null)
                {
                    if (unit.bufListDetail.GetActivatedBufList().Find(x => x is CC_Barrier) == null)
                        unit.bufListDetail.AddBuf(new CC_Barrier(Level));
                    unit.TakeDamage((5 * Level * unit.MaxHp) / 100);
                }
            }
        }
        public class CC_Barrier: BattleUnitBuf
        {
            private readonly int level;
            public CC_Barrier(int level)
            {
                this.level = level;
            }
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { breakRate=20+level*10};
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Pluto_Barrier) == null)
                    this.Destroy();
            }
        }
    }
    public class ContingecyContract_Pluto: ContingecyContract
    {
        public ContingecyContract_Pluto(int level)
        {
            this.Level = level;
        }
        public override ContractType Type => ContractType.Special;
        public override int SpeedDiceNumAdder() => Activate? 1 : 0;
        private bool Activate => (Singleton<StageController>.Instance.RoundTurn - 1) % 3 == 0;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!Activate)
                return;
            this.owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne<int>(703901, 703902, 703903, 703904, 703905));
        }
        public override void OnStartBattle()
        {
            if (!Activate)
                return;
            GameObject gameObject = Util.LoadPrefab("Battle/DiceAttackEffects/New/FX/Mon/Pluto/FX_Mon_Pluto_Paper");
            if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null))
                return;
            gameObject.AddComponent<AutoDestruct>().time = 1.5f;
            Pluto1st_Contract component = gameObject.GetComponent<Pluto1st_Contract>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
            component.Init();
        }
    }
}
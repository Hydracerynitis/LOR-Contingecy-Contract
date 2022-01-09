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
    public class ContingecyContract_Philip_Burn : ContingecyContract
    {
        public ContingecyContract_Philip_Burn(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1301011;
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam(string language) => new string[] {TextDataModel.GetText(Param1),(1 + 2 * Level).ToString() };
        private string Param1
        {
            get
            {
                switch (Level)
                {
                    case (1):
                        return "ui_resistance_vulnerable";
                    case (2):
                        return "ui_resistance_normal";
                    case (3):
                        return "ui_resistance_endure";
                }
                return "";
            }
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { breakRate = 50};
        }
        public class OverHeat_cc: BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _effect;
            private readonly int level;
            protected override string keywordId => "Philip_OverHit";
            public OverHeat_cc(int Level)
            {
                this.level = Level;
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1301014) != null)
                    return;
                this._effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("Philip/Philip_Aura_Body", 1f, owner.view, owner.view);
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Battle/Philip_Hit");
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = 1 + 2 * level
                });
            }
            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                switch (level)
                {
                    case 1:
                        return AtkResist.Vulnerable;
                    case 2:
                        return AtkResist.Normal;
                    case 3:
                        return AtkResist.Endure;
                }
                return AtkResist.Weak;
            }
            public override void Destroy()
            {
                base.Destroy();
                if (!((UnityEngine.Object)this._effect != (UnityEngine.Object)null))
                    return;
                this._effect.ManualDestroy();
                this._effect = (Battle.CreatureEffect.CreatureEffect)null;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        
    }
    public class ContingecyContract_Philip_Silence : ContingecyContract
    {
        public ContingecyContract_Philip_Silence(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1301021;
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam(string language) => new string[] { AttackPatternText,(20 * Level).ToString() };
        private string AttackPatternText => TextDataModel.GetText("Philip_Silence_param"+Level.ToString());
        public override int SpeedDiceNumAdder()
        {
            return 1;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            int num = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count - 3;
            this.owner.allyCardDetail.ExhaustAllCards();
            this.owner.allyCardDetail.AddNewCard(703121).SetCurrentCost(0);
            for(int i = 1; i < 3; i++)
            {
                if (i < Level)
                    this.owner.allyCardDetail.AddNewCard(703123);
                else
                    this.owner.allyCardDetail.AddNewCard(703122);
            }
            for (int index = 0; index < num; ++index)
                this.owner.allyCardDetail.AddNewCard(703121).SetCurrentCost(0);
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate=20*Level,breakRate=20*Level};
        }
    }
    public class ContingecyContract_Philip: ContingecyContract
    {
        private Queue<int> Priority;
        private int phase;
        private int pattern;
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1301011;
        public override ContractType Type => ContractType.Special;
        public ContingecyContract_Philip(int level)
        {
            this.Level = level;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.passiveDetail.PassiveList.Exists(x => x is PassiveAbility_1301012))
                phase = 1;
            if (self.passiveDetail.PassiveList.Exists(x => x is PassiveAbility_1301013))
                phase = 2;
            if (self.passiveDetail.PassiveList.Exists(x => x is PassiveAbility_1301014))
                phase = 3;
            pattern = 0;
        }
        private void GetCard(int extra)
        {
            if (phase == 1)
            {
                switch (pattern)
                {
                    case 0:
                        this.owner.allyCardDetail.AddNewCard(703112).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703112).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 1:
                        this.owner.allyCardDetail.AddNewCard(703112).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703112).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 2:
                        this.owner.allyCardDetail.AddNewCard(703119).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703119).SetPriorityAdder(Priority.Dequeue());
                        break;
                }
                for (int index = 0; index < extra; ++index)
                    this.owner.allyCardDetail.AddNewCard(703114).SetPriorityAdder(Priority.Dequeue());
                pattern += 1;
                pattern %= 3;
            }
            if (phase == 2)
            {
                switch (pattern)
                {
                    case 0:
                        this.owner.allyCardDetail.AddNewCard(703118).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 1:
                        this.owner.allyCardDetail.AddNewCard(703116).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 2:
                        this.owner.allyCardDetail.AddNewCard(703118).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 3:
                        this.owner.allyCardDetail.AddNewCard(703117).SetPriorityAdder(Priority.Dequeue());
                        break;
                }
                for (int index = 0; index < extra; ++index)
                    this.owner.allyCardDetail.AddNewCard(703118).SetPriorityAdder(Priority.Dequeue());
                pattern += 1;
                pattern %= 4;
            }
            if (phase == 3)
            {
                switch (pattern)
                {
                    case 0:
                        this.owner.allyCardDetail.AddNewCard(703110).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703119).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703114).SetPriorityAdder(Priority.Dequeue());
                        break;
                    case 1:
                        this.owner.allyCardDetail.AddNewCard(703116).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703119).SetPriorityAdder(Priority.Dequeue());
                        this.owner.allyCardDetail.AddNewCard(703114).SetPriorityAdder(Priority.Dequeue());
                        break;
                }
                for (int index = 0; index < extra; ++index)
                    this.owner.allyCardDetail.AddNewCard(703118).SetPriorityAdder(Priority.Dequeue());
                pattern += 1;
                pattern %= 2;
            }
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundStart();
            if(phase==1 && this.owner.IsBreakLifeZero())
            {
                PassiveAbilityBase contract = new ContingecyContract_Philip(Level)
                {
                    name = name,
                    desc = desc,
                    rare = Rarity.Unique
                };
                this.owner.passiveDetail.AddPassive(contract);
                this.owner.passiveDetail.DestroyPassive(this);
            }
            if (phase == 2 && this.owner.IsBreakLifeZero())
            {
                PassiveAbilityBase contract = new ContingecyContract_Philip(Level)
                {
                    name = name,
                    desc = desc,
                    rare = Rarity.Unique
                };
                owner.passiveDetail.AddPassive(contract);
                owner.passiveDetail.DestroyPassive(this);
            }
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStart();
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            int num = this.owner.Book.GetSpeedDiceRule(owner).Roll(owner).Count - 4;
            if (phase == 2)
                num += 1;
            if (phase == 3)
                num -= 1;
            this.owner.allyCardDetail.ExhaustAllCards();
            this.owner.allyCardDetail.AddNewCard(703117).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703115).SetPriorityAdder(Priority.Dequeue());
            this.GetCard(num);
        }
    }
}

﻿using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Damage : ContingecyContract
    {
        public ContingecyContract_Damage(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (25*Level).ToString()};
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 25 * Level });
            }
        }
    }
    public class ContingecyContract_BreakDamage : ContingecyContract
    {
        public ContingecyContract_BreakDamage(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (25 * Level).ToString() };
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = 25 * Level });
            }
        }
    }
    public class ContingecyContract_Power : ContingecyContract
    {
        public ContingecyContract_Power(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { Level.ToString() };
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = Level });
        }
    }
    public class ContingecyContract_Hp : ContingecyContract
    {
        public ContingecyContract_Hp(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (25 * Level).ToString() };
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate=25*Level};
        }
    }
    public class ContingecyContract_Bp : ContingecyContract
    {
        public ContingecyContract_Bp(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (25 * Level).ToString() };
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { breakRate = 25 * Level };
        }
    }
    public class ContingecyContract_NoBreak : ContingecyContract
    {
        public ContingecyContract_NoBreak(int level)
        {
            this.Level = level;
        }
        public override bool DontChangeResistByBreak() => true;
    }
    public class ContingecyContract_NoDebuff : ContingecyContract
    {
        public ContingecyContract_NoDebuff(int level)
        {
            this.Level = level;
        }
    }
    public class ContingecyContract_Quick: ContingecyContract
    {
        public ContingecyContract_Quick(int level)
        {
            Level = level;
        }
        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return 10000;
        }
    }
    public class ContingecyContract_Angry: ContingecyContract
    {
        public ContingecyContract_Angry(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { Level.ToString(), Level.ToString() };
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.emotionDetail.EmotionLevel < Level)
            {
                self.emotionDetail.SetEmotionLevel(Level);
                self.cardSlotDetail.RecoverPlayPoint(self.emotionDetail.MaxPlayPointAdderByLevel());
            }    
            
        }
    }
    public class ContingecyContract_SpeedDice : ContingecyContract
    {
        public ContingecyContract_SpeedDice(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { GetParam(language) };
        private string GetParam(string language)
        {
            string s = "";
            if (Level >= 1)
                s = StaticDataManager.GetParam("SpeedDice_param1", language);
            if (Level >= 2)
                s = StaticDataManager.GetParam("SpeedDice_param2", language);
            if (Level >= 3)
                s = StaticDataManager.GetParam("SpeedDice_param3", language);
            return s;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.allyCardDetail.DrawCards(1);
        }
    }
}

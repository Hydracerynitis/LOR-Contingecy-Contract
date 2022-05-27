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
    public class ContingecyContract_Roland3rd_Unity : ContingecyContract
    {
        private int pattern=0;
        public ContingecyContract_Roland3rd_Unity(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { (Level-1).ToString() };
        public override int SpeedDiceNumAdder()
        {
            if (!owner.bufListDetail.HasBuf<BattleUnitBuf_SpiritLink>() || StageController.Instance.EnemyStageManager is EnemyTeamStageManager_BlackSilence roland && roland.thirdPhaseElapsed==2)
                return Level - 1;
            return base.SpeedDiceNumAdder();
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60007 || EnemyId == 60107;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            if (BattleObjectManager.instance.GetAliveList(owner.faction).Count <= 1)
                return;
            if (owner.UnitData.unitData.EnemyUnitId == 60007)
                SetRoland();
            else
                SetAngelica();
            pattern++;
            pattern %= 3;
        }
        public void SetRoland()
        {
            switch (pattern)
            {
                case 0:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705203).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705204).SetPriorityAdder(74);
                    break;
                case 1:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705204).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705205).SetPriorityAdder(74);
                    break;
                case 2:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705203).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705205).SetPriorityAdder(74);
                    break;
            }
        }
        public void SetAngelica()
        {
            switch (pattern)
            {
                case 0:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705213).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705214).SetPriorityAdder(74);
                    break;
                case 1:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705214).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705215).SetPriorityAdder(74);
                    break;
                case 2:
                    if (Level >= 2)
                        owner.allyCardDetail.AddNewCard(705213).SetPriorityAdder(75);
                    if (Level >= 3)
                        owner.allyCardDetail.AddNewCard(705215).SetPriorityAdder(74);
                    break;
            }
        }
    }
    public class ContingecyContract_Roland3rd_Waltz : ContingecyContract
    {
        public ContingecyContract_Roland3rd_Waltz(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] {(4-Level).ToString() };
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60007 || EnemyId == 60107;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.target != null)
            {
                if (owner.UnitData.unitData.EnemyUnitId == 60007)
                    BlackWaltz.AddBuff(behavior.card.target,Level);
                else
                    WhiteWaltz.AddBuff(behavior.card.target, Level);
            }
        }
        public class WaltzDecay : BattleUnitBuf
        {
            protected int Level;
            public WaltzDecay(int level)
            {
                Level = level;
            }
            public override string bufActivatedText => TextDataModel.GetText("WaltzDecay_text",stack, (4 - Level).ToString() );
            public override void OnRoundEndTheLast()
            {
                if(StageController.Instance.EnemyStageManager is EnemyTeamStageManager_BlackSilence roland && roland.curPhase != EnemyTeamStageManager_BlackSilence.Phase.THIRD)
                    Destroy();
                else
                {
                    if(_owner.bufListDetail.GetActivatedBufList().Find(x => x is WaltzDecay && x.bufActivatedName!= this.bufActivatedName) is WaltzDecay decay)
                    {
                        if (Math.Abs(decay.stack - this.stack) <= 4 - Level)
                        {
                            stack -= 1;
                            if (stack <= 0)
                                Destroy();
                            return;
                        }
                    }
                    _owner.TakeDamage(stack);
                    _owner.TakeBreakDamage(stack);
                    stack -= 1;
                    if (stack <= 0)
                        Destroy();
                }
            }
        }
        public class BlackWaltz: WaltzDecay
        {
            public BlackWaltz(int level) : base(level)
            {
            }
            public override string keywordIconId => "BlackEnergy";
            public override string keywordId => "BlackDecay";
            public static void AddBuff(BattleUnitModel unit, int level)
            {
                if(unit.bufListDetail.GetActivatedBufList().Find(x => x is BlackWaltz) is BlackWaltz black)
                    black.stack+=1;
                else
                {
                    black = new BlackWaltz(level) { stack = 1 };
                    unit.bufListDetail.AddBuf(black);
                }
            }
        }
        public class WhiteWaltz : WaltzDecay
        {
            public WhiteWaltz(int level) : base(level)
            {
            }
            public override string keywordIconId => "WhiteEnergy";
            public override string keywordId => "WhiteDecay";
            public static void AddBuff(BattleUnitModel unit, int level)
            {
                if (unit.bufListDetail.GetActivatedBufList().Find(x => x is WhiteWaltz) is WhiteWaltz white)
                    white.stack += 1;
                else
                {
                    white = new WhiteWaltz(level) { stack = 1 };
                    unit.bufListDetail.AddBuf(white);
                }
            }
        }
    }
    public class ContingecyContract_Roland3rd: ContingecyContract
    {
        public ContingecyContract_Roland3rd(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60007 || EnemyId == 60107;
        }
        public override void OnStartBattle()
        {
            BattlePlayingCardDataInUnitModel card = owner.cardSlotDetail.cardAry.Find(x => x.card.GetID() == 705201 || x.card.GetID() == 705211);
            if(card!=null)
            {
                if (card.target.bufListDetail.HasBuf<ForbidEnergy>())
                {
                    card.target.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_WhiteEnergy));
                    card.target.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_BlackEnergy));
                }
                else if(card.target.faction==Faction.Player)
                    card.target.bufListDetail.AddBuf(new ForbidEnergy());
            }
                
        }
        public override void OnRoundStart()
        {
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_BlackMark)));
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => !x.bufListDetail.HasBuf<ForbidEnergy>())).bufListDetail.AddBuf(new BattleUnitBuf_BlackMark());
        }
        public class ForbidEnergy: BattleUnitBuf
        {
            public override string keywordId => "ForbidEnergy";
            public override string keywordIconId => "ForbidRecovery";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 3;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                --this.stack;
                if (this.stack > 0)
                    return;
                this.Destroy();
            }
        }
    }
   
}

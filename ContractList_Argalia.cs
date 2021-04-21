using System;
using System.IO;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Argalia_Resonance : ContingecyContract
    {
        public ContingecyContract_Argalia_Resonance(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (Level - 1).ToString() ,(2*Level).ToString()};
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            List<PassiveAbilityBase> list = self.passiveDetail.PassiveList;
            list.RemoveAt(3);
            list.Insert(3, new ResonaceUpgrade(this.owner, Level));
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)self.passiveDetail, (object)list);
        }
        public class ResonaceUpgrade: PassiveAbilityBase
        {
            private int Level;
            public ResonaceUpgrade(BattleUnitModel owner, int level)
            {
                this.owner = owner;
                this.Level = level;
                this.name = Singleton<PassiveDescXmlList>.Instance.GetName(20210331);
                this.desc = string.Format(Singleton<PassiveDescXmlList>.Instance.GetDesc(20210331), new string[] { (Level - 1).ToString(), (2 * Level).ToString() });
                this.rare = Rarity.Unique;
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                if (curCard.card.GetSpec().Ranged != CardRange.Near)
                    return;
                BattleUnitBuf activatedBuf = curCard.target.bufListDetail.GetActivatedBuf(KeywordBuf.Vibrate);
                if (activatedBuf == null || Mathf.Abs(curCard.speedDiceResultValue - activatedBuf.stack) > Level-1)
                    return;
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    power = Level*2
                });
                this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
                this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1);
                curCard.OnActivateResonance();
            }
        }
    }
    public class ContingecyContract_Argalia_TieUP : ContingecyContract
    {
        private int count;
        public ContingecyContract_Argalia_TieUP(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (20+10*Level).ToString(),(7-Level).ToString()};
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() {hpRate=20+10*Level,breakRate=20+10*Level };
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Slash && this.owner.hp>25)
                return AtkResist.Endure;
            return origin;
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Hit && this.owner.hp > 25)
                return AtkResist.Endure;
            return origin;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            count = 0;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            count += 1;
            if (count >= 7 - Level)
            {
                count -= 7 - Level;
                this.owner.RecoverHP(10);
                this.owner.breakDetail.RecoverBreak(10);
            }
        }
    }
    public class ContingecyContract_Argalia : ContingecyContract
    {
        private bool activated;
        private int index;
        private int phase;
        private Queue<int> Priority;
        public ContingecyContract_Argalia(int level)
        {
            this.Level = level;
            phase = 1;
            activated = false;
        }
        public override int SpeedDiceNumAdder()
        {
            if (phase==3)
                return 0;
            return Singleton<StageController>.Instance.RoundTurn >= 4 ? 1 : 0;
        }
        public override ContractType Type => ContractType.Special;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if(this.owner.hp<=375 && phase == 1)
            {
                phase = 2;
            }
            if(this.owner.hp<=25 && phase == 2)
            {
                phase = 3;
                RecoverBreak();
            }
        }
        public override void OnAfterRollSpeedDice()
        {
            index = 0;
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            this.owner.allyCardDetail.ExhaustAllCards();
            if (phase == 1)
            {
                CheckFurioso();
                SetCardPhase1(GetOpponentVibeStack());
            }
            if (phase == 2)
            {
                CheckFurioso();
                SetCardPhase2(GetOpponentVibeStack());
            }
            if (phase == 3)
                this.owner.allyCardDetail.AddNewCard(703021).SetPriorityAdder(Priority.Dequeue());
        }
        private void SetCardPhase1(int vibrate)
        {
            for (;index<this.owner.speedDiceResult.Count;index++)
            {
                if (this.owner.speedDiceResult[index].value == vibrate)
                    this.owner.allyCardDetail.AddNewCard(703031).SetPriorityAdder(Priority.Dequeue());
                else
                {
                    switch (index)
                    {
                        case 0:
                            if (vibrate > 5)
                                this.owner.allyCardDetail.AddNewCard(703002).SetPriorityAdder(Priority.Dequeue());
                            else if (vibrate < 4)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703005).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 1:
                            this.owner.allyCardDetail.AddNewCard(703003).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 2:
                            if (vibrate < 1)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703004).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 3:
                            if (Singleton<StageController>.Instance.RoundTurn>1)
                                this.owner.allyCardDetail.AddNewCard(703004).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 4:
                            if (vibrate > 5)
                                this.owner.allyCardDetail.AddNewCard(703002).SetPriorityAdder(Priority.Dequeue());
                            else if (vibrate < 4)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703005).SetPriorityAdder(Priority.Dequeue());
                            continue;
                    }
                    this.owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne<int>(703004, 703005));
                }
            }
        }
        private void SetCardPhase2(int vibrate)
        {
            for (; index < this.owner.speedDiceResult.Count; index++)
            {
                if (this.owner.speedDiceResult[index].value == vibrate)
                    this.owner.allyCardDetail.AddNewCard(703031).SetPriorityAdder(Priority.Dequeue());
                else
                {
                    switch (index)
                    {
                        case 0:
                            if (vibrate > 5)
                                this.owner.allyCardDetail.AddNewCard(703002).SetPriorityAdder(Priority.Dequeue());
                            else if (vibrate < 4)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703011).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 1:
                            this.owner.allyCardDetail.AddNewCard(703003).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 2:
                            if (vibrate < 1)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703004).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 3:
                            if (Singleton<StageController>.Instance.RoundTurn%2==0)
                                this.owner.allyCardDetail.AddNewCard(703012).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703011).SetPriorityAdder(Priority.Dequeue());
                            continue;
                        case 4:
                            if (vibrate > 5)
                                this.owner.allyCardDetail.AddNewCard(703002).SetPriorityAdder(Priority.Dequeue());
                            else if (vibrate < 4)
                                this.owner.allyCardDetail.AddNewCard(703001).SetPriorityAdder(Priority.Dequeue());
                            else
                                this.owner.allyCardDetail.AddNewCard(703005).SetPriorityAdder(Priority.Dequeue());
                            continue;
                    }
                    this.owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne<int>(703004, 703005,703011));
                }
            }
        }
        private int GetOpponentVibeStack()
        {
            int num = 0;
            List<BattleUnitModel> aliveListOpponent = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction);
            if (aliveListOpponent.Count > 0)
            {
                BattleUnitBuf activatedBuf = aliveListOpponent[0].bufListDetail.GetActivatedBuf(KeywordBuf.Vibrate);
                if (activatedBuf != null)
                    num = activatedBuf.stack;
            }
            return num;
        }
        private void RecoverBreak()
        {
            if (this.owner.turnState == BattleUnitTurnState.BREAK)
                this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
            this.owner.breakDetail.nextTurnBreak = false;
            this.owner.breakDetail.RecoverBreakLife(1);
            this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
        }
        private void CheckFurioso()
        {
            BattleUnitModel Roland = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction)[0];
            if (Roland.passiveDetail.PassiveList.Find((x => x is PassiveAbility_10012)) is PassiveAbility_10012 passiveAbility10012 && passiveAbility10012.IsActivatedSpecialCard() && !activated)
            {
                this.owner.allyCardDetail.AddNewCard(703021).SetPriorityAdder(Priority.Dequeue());
                index += 1;
                RecoverBreak();
                activated = true;
            }
            else
                activated = false;
        }
    }
}
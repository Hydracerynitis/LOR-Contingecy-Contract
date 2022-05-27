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
    public class ContingecyContract_Jaeheon_Thread : ContingecyContract
    {
        public ContingecyContract_Jaeheon_Thread(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1307011;
        public override string[] GetFormatParam(string language) => new string[] { Level.ToString(), (Level - 1).ToString() };
        public override bool isInvincibleHp => BattleObjectManager.instance.GetAliveList(this.owner.faction).Count > 1;
        public override bool isInvincibleBp => BattleObjectManager.instance.GetAliveList(this.owner.faction).Count > 1;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (Singleton<StageController>.Instance.RoundTurn > 4)
                return;
            int puppetId = -1;
            switch (Singleton<StageController>.Instance.RoundTurn)
            {
                case 1:
                    puppetId = 1307021;
                    break;
                case 2:
                    puppetId = 1307031;
                    break;
                case 3:
                    puppetId = 1307041;
                    break;
                case 4:
                    puppetId = 1307051;
                    break;
            }
            BattleUnitModel owner = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == puppetId);
            if (owner == null)
                return;
            BattleUnitBuf thread = owner.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonPuppetThread);
            thread.stack += (Level - 1);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Type != BehaviourType.Standby)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = Level });
        }
    }
    public class ContingecyContract_Jaeheon_Puppet : ContingecyContract
    {
        public ContingecyContract_Jaeheon_Puppet(int level)
        {
            Level = level;
            waitThreads = new List<WaitThread>();
        }
        private List<WaitThread> waitThreads;
        public override string[] GetFormatParam(string language) => new string[] { (4 - Level).ToString(), (10+30*Level).ToString() };
        private bool IsJaeheon => this.owner.UnitData.unitData.EnemyUnitId == new LorId(1307011);
        private bool HasThread(BattleUnitModel unit) => unit.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonPuppetThread) != null;
        public override int SpeedDiceNumAdder() => HasThread(this.owner) ? 1 : 0;
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if (!HasThread(this.owner))
                return;
            this.owner.allyCardDetail.ExhaustCardInHand(GetSpecialCardId());
            this.owner.allyCardDetail.ExhaustCardInDeck(new LorId(GetSpecialCardId()));
            this.owner.allyCardDetail.ReturnAllToDeck();
            this.owner.allyCardDetail.DrawCards(this.owner.allyCardDetail.maxHandCount - 2);
            BattleDiceCardModel card=this.owner.allyCardDetail.AddNewCard(GetSpecialCardId());
            card.temporary = true;
            card.SetPriorityAdder(200);
            card = this.owner.allyCardDetail.AddNewCard(GetSpecialCardId());
            card.temporary = true;
            card.SetPriorityAdder(200);
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if(!IsJaeheon)
                return base.GetStatBonus(owner);
            return new StatBonus() { breakRate = 10 + 30 * Level };
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (!IsJaeheon || Singleton<StageController>.Instance.RoundTurn<5)
                return;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x != this.owner))
            {
                if (!HasThread(unit) && !waitThreads.Exists(x => x.unitID == unit.UnitData.unitData.EnemyUnitId))
                    waitThreads.Add(new WaitThread() { unitID = unit.UnitData.unitData.EnemyUnitId, waitRound = 4-Level });
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (HasThread(this.owner) && Singleton<StageController>.Instance.RoundTurn == 1)
            {
                this.owner.OnRoundStartOnlyUI();
                this.owner.RollSpeedDice();
            }
            if (waitThreads.Count <= 0)
                return;
            foreach (WaitThread waitThread in waitThreads)
            {
                waitThread.waitRound -= 1;
                if (waitThread.waitRound == 0)
                {
                    BattleUnitModel owner = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == waitThread.unitID);
                    if (owner == null)
                        return;
                    BattleUnitBuf_Jaeheon_PuppetThread jaeheonPuppetThread = new BattleUnitBuf_Jaeheon_PuppetThread();
                    jaeheonPuppetThread.Init(owner);
                    jaeheonPuppetThread.stack = 2;
                    owner.bufListDetail.AddBuf(jaeheonPuppetThread);
                }
            }
            waitThreads.RemoveAll(x => x.waitRound == 0);
        }
        private int GetSpecialCardId()
        {
            int num = -1;
            switch (this.owner.UnitData.unitData.EnemyUnitId.id)
            {
                case 1307021:
                    num = 703720;
                    break;
                case 1307031:
                    num = 703721;
                    break;
                case 1307041:
                    num = 703722;
                    break;
                case 1307051:
                    num = 703723;
                    break;
            }
            return num;
        }
        private class WaitThread
        {
            public LorId unitID;
            public int waitRound;
        }
    }
    public class ContingecyContract_Jaeheon : ContingecyContract
    {
        private Queue<int> Priority;
        public ContingecyContract_Jaeheon(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1307011;
        public override void OnRoundStart()
        {
            int round = Singleton<StageController>.Instance.RoundTurn;
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            this.owner.allyCardDetail.ExhaustAllCards();
            AddCard(round);
            base.OnRoundStart();
        }
        private void AddCard(int round)
        {
            if (round == 1)
            {
                this.owner.allyCardDetail.AddNewCard(703709).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703709).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703705).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703701).SetPriorityAdder(Priority.Dequeue());
                for (; this.owner.allyCardDetail.GetHand().Count < this.owner.Book.GetSpeedDiceRule(this.owner).diceNum;)
                    this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                return;
            }
            if (round % 3 == 1)
                this.owner.allyCardDetail.AddNewCard(703709).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703703).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703701).SetPriorityAdder(Priority.Dequeue());
            switch (round % 4)
            {
                case (0):
                    if (HasThreadBuf(1307051))
                        this.owner.allyCardDetail.AddNewCard(703708).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703703).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307041))
                        this.owner.allyCardDetail.AddNewCard(703707).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307031))
                        this.owner.allyCardDetail.AddNewCard(703706).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307021))
                        this.owner.allyCardDetail.AddNewCard(703705).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (3):
                    if (HasThreadBuf(1307041))
                        this.owner.allyCardDetail.AddNewCard(703707).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703703).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307031))
                        this.owner.allyCardDetail.AddNewCard(703706).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307021))
                        this.owner.allyCardDetail.AddNewCard(703705).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307051))
                        this.owner.allyCardDetail.AddNewCard(703708).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (2):
                    if (HasThreadBuf(1307031))
                        this.owner.allyCardDetail.AddNewCard(703706).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703703).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307021))
                        this.owner.allyCardDetail.AddNewCard(703705).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307051))
                        this.owner.allyCardDetail.AddNewCard(703708).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307041))
                        this.owner.allyCardDetail.AddNewCard(703707).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (1):
                    if (HasThreadBuf(1307021))
                        this.owner.allyCardDetail.AddNewCard(703705).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703703).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307051))
                        this.owner.allyCardDetail.AddNewCard(703708).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307041))
                        this.owner.allyCardDetail.AddNewCard(703707).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703704).SetPriorityAdder(Priority.Dequeue());
                    if (HasThreadBuf(1307031))
                        this.owner.allyCardDetail.AddNewCard(703706).SetPriorityAdder(Priority.Dequeue());
                    else
                        this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
            for (; this.owner.allyCardDetail.GetHand().Count < this.owner.Book.GetSpeedDiceRule(this.owner).diceNum;)
                this.owner.allyCardDetail.AddNewCard(703702).SetPriorityAdder(Priority.Dequeue());
        }
        private bool HasThreadBuf(int unitId)
        {
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            aliveList.Remove(this.owner);
            bool flag = false;
            BattleUnitModel battleUnitModel = aliveList.Find(x => x.UnitData.unitData.EnemyUnitId == unitId);
            if (battleUnitModel != null && battleUnitModel.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Jaeheon_PuppetThread) != null)
                flag = true;
            return flag;
        }
    }
}
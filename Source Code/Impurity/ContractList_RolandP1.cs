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
    public class ContingecyContract_Roland1st_Furioso : ContingecyContract
    {
        private static int[] Arsenal = new int[]{702001,702002,702003,702004,702005,702006,702007,702008, 702009};
        private Queue<int> Priority=new Queue<int>();
        private int StrongProc = 0;
        public ContingecyContract_Roland1st_Furioso(int level)
        {
            Level = level;
        }
        public override int SpeedDiceNumAdder()
        {
            int num = Level;
            if (owner.emotionDetail.EmotionLevel >= 3 && Level >= 2)
                num -= 1;
            if (owner.emotionDetail.EmotionLevel >= 4 && Level >= 3)
                num -= 1;
            return num;
        }
        public override string[] GetFormatParam(string language) => new string[] { StaticDataManager.GetParam("Roland1st_Furioso_param" + Level.ToString(), language), StaticDataManager.GetParam("Philip_Silence_param" + Level.ToString(), language)};
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId== 60005;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.slotOrder != 5)
                return;
            curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus(){ power = 2 });
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if(self.passiveDetail.PassiveList.Find(x => x is PassiveAbility_170005) is PassiveAbility_170005 passive)
            {
                passive.name = TextDataModel.GetText("Speed_Furioso");
                passive.desc = StaticDataManager.GetParam("Roland1st_Furioso_param" + Level.ToString(), TextDataModel.CurrentLanguage);
            }
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            Priority.Clear();
            owner.allyCardDetail.ExhaustAllCards();
            for (int i = 100; i > 0; i -= 10)
                Priority.Enqueue(i);
            int num = owner.equipment.book.GetSpeedDiceRule(owner).diceNum-1;
            int cursor = 0;
            List<int> cards = new List<int>(Arsenal);
            if (StageController.Instance.RoundTurn % 4 == 0)
            {
                SetCard_Furioso();
                cursor += 4;
                cards.Remove(702009);
            }
            int LastCard = RandomUtil.SelectOne(702001, 702003, 702004);
            cards.Remove(LastCard);
            for (; num > cards.Count;)
                cards.Add(RandomUtil.SelectOne(702001, 702003, 702004));
            for (; cards.Count > num && cards.Exists(x => x < 702007);)
                cards.Remove(RandomUtil.SelectOne(cards.FindAll(x=> x< 702007)));
            List<int> ProcCard = new List<int>();
            for (int i = num; i >= 3; i -= 3)
            {
                List<int> LowTier = cards.FindAll(x => x < 702007);
                List<int> HighTier = cards.FindAll(x => x >= 702007);
                if ((StrongProc >= 3 - Level || LowTier.Count<=0) && HighTier.Count>0)
                {
                    int card = RandomUtil.SelectOne(HighTier);
                    ProcCard.Add(card);
                    cards.Remove(card);
                    StrongProc = 0;
                }
                else if(LowTier.Count>0)
                {
                    int card = RandomUtil.SelectOne(LowTier);
                    ProcCard.Add(card);
                    cards.Remove(card);
                    StrongProc++;
                }
            }
            for (; cursor < num; cursor++)
            {
                if (cursor%3 == 2 && ProcCard.Count > 0)
                {
                    int card = RandomUtil.SelectOne(ProcCard);
                    owner.allyCardDetail.AddTempCard(card).SetPriorityAdder(Priority.Dequeue());
                    ProcCard.Remove(card);
                }
                else
                {
                    int card;
                    if (cards.Count > 0)
                    {
                        card = RandomUtil.SelectOne(cards);
                        owner.allyCardDetail.AddTempCard(card).SetPriorityAdder(Priority.Dequeue());
                        cards.Remove(card);
                    }
                    else if (ProcCard.Count > 0)
                    {
                        card = RandomUtil.SelectOne(ProcCard);
                        owner.allyCardDetail.AddTempCard(card).SetPriorityAdder(Priority.Dequeue());
                        ProcCard.Remove(card);
                    }
                }
            }
            owner.allyCardDetail.AddTempCard(LastCard).SetPriorityAdder(Priority.Dequeue());
            owner.allyCardDetail.GetAllDeck().ForEach(x => x.SetCostToZero());
            owner.OnRoundStartOnlyUI();
        }
        private void SetCard_Furioso()
        {
            owner.allyCardDetail.AddTempCard(702010).SetPriorityAdder(Priority.Dequeue());
            owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
            owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
            owner.allyCardDetail.AddTempCard(702010).SetPriorityAdder(Priority.Dequeue());
        }
    }
    public class ContingecyContract_Roland1st_Durandal : ContingecyContract
    {
        private Queue<int> Priority = new Queue<int>();
        public ContingecyContract_Roland1st_Durandal(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] {  GetParam(language) };
        private string GetParam(string language)
        {
            string s = "";
            if (Level >= 1)
                s += StaticDataManager.GetParam("Roland1st_Durandal_param1", language);
            if (Level >= 2)
                s += StaticDataManager.GetParam("Roland1st_Durandal_param2", language);
            if (Level >= 3)
                s += StaticDataManager.GetParam("Roland1st_Durandal_param3", language);
            return s;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60005;
        }
        public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
        {
            if (Level >= 2 && buf.bufType == KeywordBuf.Strength)
                return 1;
            return base.OnGiveKeywordBufByCard(buf, stack, target);
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            Priority.Clear();
            owner.allyCardDetail.ExhaustAllCards();
            for (int i = 100; i > 0; i -= 10)
                Priority.Enqueue(i);
            switch (StageController.Instance.RoundTurn % 4)
            {
                case (1):
                    owner.allyCardDetail.AddTempCard(702008).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702003).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702001).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (2):
                    owner.allyCardDetail.AddTempCard(702002).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702004).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702001).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (3):
                    owner.allyCardDetail.AddTempCard(702007).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702005).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702006).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    break;
                case (0):
                    owner.allyCardDetail.AddTempCard(702010).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddTempCard(702009).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
            if (this.owner.Book.GetSpeedDiceRule(this.owner).diceNum < 6)
                return;
            int num = this.owner.Book.GetSpeedDiceRule(this.owner).diceNum - 5;
            List<int> list = new List<int>() { 702001, 702003, 702004 };
            for (int index = 0; index < num; ++index)
            {
                if (list.Count == 0)
                    list = new List<int>() { 702001, 702003, 702004 };
                int id = RandomUtil.SelectOne<int>(list);
                owner.allyCardDetail.AddTempCard(id).SetPriorityAdder(Priority.Dequeue());
                list.Remove(id);
            }
            owner.allyCardDetail.GetAllDeck().ForEach(x => x.SetCostToZero());
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if (Level >= 3)
            {
                foreach (BattleDiceCardModel card in owner.allyCardDetail.GetHand())
                {
                    if (card.GetID() == 702009)
                        DeepCopyUtil.EnhanceCard(card, 2, 2);
                }
            }
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(705001);
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour.Copy();
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            this.owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);
        }
    }
    public class ContingecyContract_Roland1st : ContingecyContract
    {
        public ContingecyContract_Roland1st(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60005;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() {hpAdder=300 };
        }
    }
}

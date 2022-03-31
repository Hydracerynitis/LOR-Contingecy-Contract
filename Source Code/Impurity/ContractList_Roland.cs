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
    public class ContingecyContract_Roland4th_BlackSilence : ContingecyContract
    {
        public ContingecyContract_Roland4th_BlackSilence(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] {(150* Level-50).ToString(),(75*Level-25).ToString() };
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60008;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (owner.UnitData.unitData.EnemyUnitId == 60008)
                return new StatBonus() { hpAdder = 150 * Level - 50, breakGageAdder = 75 * Level - 25 };
            return base.GetStatBonus(owner);
        }
    }
    public class ContingecyContract_Roland4th_Servant : ContingecyContract
    {
        public ContingecyContract_Roland4th_Servant(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam(string language) => new string[] { GetParam(language) };
        private string GetParam(string language)
        {
            string s = "";
            if (Level >= 1)
                s += StaticDataManager.GetParam("Roland4th_Servant_param1", language);
            if (Level >= 2)
                s += StaticDataManager.GetParam("Roland4th_Servant_param2", language);
            if (Level >= 3)
                s += StaticDataManager.GetParam("Roland4th_Servant_param3", language);
            return s;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId.IsBasic() && EnemyId.id >= 60208 && EnemyId.id <= 60238;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate=100, breakRate=100};
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.bufListDetail.AddBuf(new Nail());
            if (Level >= 2)
                self.bufListDetail.AddBuf(new Guilt());
            if (Level >= 3)
                self.bufListDetail.AddBuf(new Blizzard());
        }
        public class Nail: BattleUnitBuf
        {
            public override string keywordId => "Servent_Hammer";
            public override string keywordIconId => "KeterFinal_SilenceGirl_Nail";
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (behavior.Detail != BehaviourDetail.Penetrate)
                    return;
                behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Nail, 2);
                behavior.card.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Slientgirl_Volt");
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
        }
        public class Guilt : BattleUnitBuf
        {
            public override string keywordId => "Servent_Guilt";
            public override string keywordIconId => "KeterFinal_SilenceGirl_Gaze_Attacked";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                {
                    if (unit.allyCardDetail.GetHand().Find(x => x.GetID() == Tools.MakeLorId(1)) is BattleDiceCardModel card)
                        return;
                    unit.allyCardDetail.AddNewCard(Tools.MakeLorId(1));
                }
                stack = 0;            
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                if (behavior.owner.allyCardDetail.GetHand().Find(x => x.GetID() == Tools.MakeLorId(1)) is BattleDiceCardModel card)
                    return card.GetCost();
                return base.GetDamageReduction(behavior);
            }
            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                if (behavior.card.target.allyCardDetail.GetHand().Find(x => x.GetID() == Tools.MakeLorId(1)) is BattleDiceCardModel card)
                    card.AddCost(-1);
            }
            public override void OnDie()
            {
                base.OnDie();
                if (BattleObjectManager.instance.GetAliveList(_owner.faction).Exists(x => x != _owner && x.bufListDetail.HasBuf<Guilt>()))
                    return;
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                {
                    BattleDiceCardModel card = unit.allyCardDetail.GetHand().Find(x => x.GetID() == Tools.MakeLorId(1));
                    if (card != null)
                        unit.allyCardDetail.ExhaustACardAnywhere(card);
                }
            }
        }
        public class Blizzard : BattleUnitBuf
        {
            public override string keywordId => "Servent_Blizzard";
            public override string keywordIconId => "SnowQueen_Stun";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => x.IsActionable())).bufListDetail.AddBuf(new SnowQueen_Stun());
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = GameObject.Instantiate(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
                stack = 0;
            }
            public class SnowQueen_Stun : BattleUnitBuf
            {
                private Battle.CreatureEffect.CreatureEffect _aura;
                public override bool Hide => true;
                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 1);
                    if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                        return;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
                }
                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (_aura != null)
                    {
                        GameObject.Destroy(_aura.gameObject);
                        this._aura = null;
                        this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    }
                    this.Destroy();
                }
            }
        }
    }
    public class ContingecyContract_Roland4th : ContingecyContract
    {
        private Dictionary<BehaviourDetail, int> HitDic = new Dictionary<BehaviourDetail, int>();
        public ContingecyContract_Roland4th(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 60008;
        }
        public override bool isImmuneByFarAtk => true;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            int min = 2;
            int max = 0;
            if (this.owner.emotionDetail.EmotionLevel >= 3)
                max = 1;
            int power = 1;
            if (IsDefenseDice(behavior.Detail))
            {
                power += 1;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = power, min=min, max=max });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card == null || behavior.card.card.GetSpec().Ranged != CardRange.Near)
                return;
            behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 1);
            behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BurnSpread, 2);
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (behavior.card.target == null || behavior.card.target.bufListDetail.GetKewordBufAllStack(KeywordBuf.Burn) <= 0)
                return;
            int damage = RandomUtil.Range(1, 2);
            behavior.card.target.TakeBreakDamage(damage, DamageType.Passive, this.owner);
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.faction != this.owner.faction)
                return;
            this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this.owner);
        }
        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            int diceMin = behavior.GetDiceMin();
            int diceMax = behavior.GetDiceMax();
            if (diceResult <= diceMin)
            {
                diceResult = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
                behavior.owner.battleCardResultLog?.SetVanillaDiceValue(diceMin);
                behavior.owner.battleCardResultLog?.SetPassiveAbility((PassiveAbilityBase)this);
                ++behavior.owner.UnitData.historyInUnit.rerollByPurpleTear;
            }
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            HitDic.Add(BehaviourDetail.Slash, 0);
            HitDic.Add(BehaviourDetail.Penetrate, 0);
            HitDic.Add(BehaviourDetail.Hit, 0);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            BehaviourDetail detail = behavior.behaviourInCard.Detail;
            if (HitDic.ContainsKey(detail))
            {
                HitDic[detail] += 1;
                return Math.Min(2, HitDic[detail] * 2);
            }
            return base.GetDamageReduction(behavior);
        }
        public override void OnRoundEnd()
        {
            HitDic[BehaviourDetail.Slash] = 0;
            HitDic[BehaviourDetail.Penetrate] = 0;
            HitDic[BehaviourDetail.Hit] = 0;
            base.OnRoundEnd();
        }
    }
    public class ContingecyContract_Roland : ContingecyContract
    {
        public ContingecyContract_Roland(int level)
        {
            Level = level;
        }
    }
    public class StageModifier_Roland : StageModifier
    {
        public StageModifier_Roland(int Level)
        {
            this.Level = Level;
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.id == 60003;
        }
        public override void Modify(ref StageClassInfo info)
        {
            foreach (StageWaveInfo wave in info.waveList)
            {
                wave.enemyUnitIdList.Clear();
                wave.enemyUnitIdList.Add(new LorId(60008));
            }
        }
    }
}

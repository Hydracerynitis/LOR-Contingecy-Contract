using System;
using System.Collections.Generic;
using UI;
using HarmonyLib;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Bremen_Self : ContingecyContract
    {
        public ContingecyContract_Bremen_Self(int level)
        {
            Level = level;
        }
        public static bool CheckEnemyId(int EnemyId) => EnemyId == 1304011;
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { Level.ToString(), GetParam() };
        private PassiveAbility_1304012 passive=null;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            passive = self.passiveDetail.PassiveList.Find(x=>x is PassiveAbility_1304012) as PassiveAbility_1304012;
        }
        public override int SpeedDiceNumAdder()
        {
            return Level;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            if (passive == null || !owner.passiveDetail.PassiveList.Contains(passive))
                return;
            switch ((int)typeof(PassiveAbility_1304012).GetField("currentPhase", AccessTools.all).GetValue(passive))
            {
                case 0:
                    GetCard(new List<int>(){ 703415, 703416});
                    break;
                case 1:
                    GetCard(new List<int>(){ 703411, 703412});
                    break;
                case 2:
                    GetCard(new List<int>(){ 703413, 703414});
                    break;
            }
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (passive.destroyed)
            {
                owner.passiveDetail.AddPassive(new ContingecyContract_Bremen_Self_2(Level));
                owner.passiveDetail.DestroyPassive(this);
            }
        }
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Bremen_Self_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Bremen_Self_param2");
            if(Level >=3)
                s += TextDataModel.GetText("Bremen_Self_param3");
            return s;
        }

        private void GetCard(List<int> ids)
        {
            if (Level >= 1)
            {
                int num = RandomUtil.SelectOne<int>(ids);
                owner.allyCardDetail.AddNewCard(num).SetPriorityAdder(700);
                ids.Remove(num);
            }
            if (Level >= 3)
                owner.allyCardDetail.AddNewCard(ids[0]).SetPriorityAdder(700);
        }
    }
    public class ContingecyContract_Bremen_Self_2 : ContingecyContract
    {
        public ContingecyContract_Bremen_Self_2(int level)
        {
            Level = level;
        }
        public override string[] GetFormatParam => new string[] { Level.ToString(), GetParam() };
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Bremen_Self_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Bremen_Self_param2");
            if (Level >= 3)
                s += TextDataModel.GetText("Bremen_Self_param3");
            return s;
        }
        public override int SpeedDiceNumAdder()
        {
            return Level;
        }
        public override void OnCreated()
        {
            base.OnCreated();
            name = Singleton<PassiveDescXmlList>.Instance.GetName(20210302) + Singleton<ContractXmlList>.Instance.GetContract("Bremen_Self").GetDesc(TextDataModel.CurrentLanguage).name;
            desc = string.Format(Singleton<ContractXmlList>.Instance.GetContract("Bremen_Self").GetDesc(TextDataModel.CurrentLanguage).desc, this.GetFormatParam);
            rare = Rarity.Unique;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            if (Level >= 1)
                owner.allyCardDetail.AddNewCard(703422).SetPriorityAdder(800);
            if (Level >= 2)
                owner.allyCardDetail.AddNewCard(703423).SetPriorityAdder(800);
            if(Level>=3)
                if (!owner.allyCardDetail.GetAllDeck().Exists(x => x.GetID() == 703425))
                    owner.allyCardDetail.AddNewCard(703425).SetPriorityAdder(800);
                else
                    owner.allyCardDetail.AddNewCard(703424).SetPriorityAdder(800);
        }
        private void Getbuff(BattleUnitModel unit)
        {
            if (Level >= 1)
                unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 2, owner);
            if (Level >= 2)
                unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 2, owner);
            if (Level >= 3)
                unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 2, owner);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card.GetID() != 703426)
                return;
            Getbuff(curCard.target);
            if (curCard.subTargets.Count <= 0)
                return;
            foreach (BattlePlayingCardDataInUnitModel.SubTarget sub in curCard.subTargets)
            {
                sub.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 10, owner);
                sub.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 5, owner);
                Getbuff(sub.target);
            }
        }
    }
    public class ContingecyContract_Bremen_Group : ContingecyContract
    {
        public ContingecyContract_Bremen_Group(int level)
        {
            Level = level;
        }
        public override ContractType Type => ContractType.Special;
        private bool isBremen => owner.UnitData.unitData.EnemyUnitId == 1304011;
        private List<BattleUnitModel> dead = new List<BattleUnitModel>();
        private int count;
        public override string[] GetFormatParam => new string[] { GetParam(), Level.ToString() };
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Bremen_Group_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Bremen_Group_param2");
            if (Level >= 3)
                s += TextDataModel.GetText("Bremen_Group_param3");
            return s;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (isBremen)
                return;
            if (Level >= 1)
                for (int i = 0; i < 3; i++)
                    self.allyCardDetail.AddNewCardToDeck(703422).SetCurrentCost(2);
            if (Level >= 2)
                for (int i = 0; i < 3; i++)
                    self.allyCardDetail.AddNewCardToDeck(703423).SetCurrentCost(2);
            if (Level >= 3)
                self.allyCardDetail.AddNewCardToDeck(703425).SetCurrentCost(4);
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStart();
            count = Level;
            if (!isBremen)
                return;
            dead.AddRange(BattleObjectManager.instance.GetList(owner.faction).FindAll(x => x.IsDead()));
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (!isBremen)
                return;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList(owner.faction).FindAll(x => x.IsDead() && dead.Contains(x)))
            {
                if (unit.UnitData.unitData.EnemyUnitId == 1304011)
                    continue;
                BattleUnitModel NewUnit=Singleton<StageController>.Instance.AddNewUnit(this.owner.faction, unit.UnitData.unitData.EnemyUnitId, unit.index);
                NewUnit.emotionDetail.SetEmotionLevel(owner.emotionDetail.EmotionLevel);
                NewUnit.allyCardDetail.DrawCards(NewUnit.Book.GetStartDraw());
                dead.Remove(unit);
            }
            int num = 0;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++);
            BattleObjectManager.instance.InitUI();
        }
        public override void OnAddKeywordBufByCardForEvent(KeywordBuf keywordBuf, int stack, BufReadyType readyType)
        {
            if (keywordBuf != KeywordBuf.Strength && keywordBuf != KeywordBuf.Endurance && keywordBuf != KeywordBuf.Quickness && keywordBuf != KeywordBuf.Protection)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x != owner);
            if (aliveList.Count <= 0 || count <= 0)
                return;
            BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
            switch (readyType)
            {
                case BufReadyType.ThisRound:
                    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(keywordBuf, stack);
                    break;
                case BufReadyType.NextRound:
                    battleUnitModel.bufListDetail.AddKeywordBufByEtc(keywordBuf, stack);
                    break;
            }
            count--;
        }
    }
    public class ContingecyContract_Bremen : ContingecyContract
    {
        public ContingecyContract_Bremen(int level)
        {
            Level = level;
        }
        private double accumulateDamage;
        public override ContractType Type => ContractType.Special;
        public static bool CheckEnemyId(int EnemyId) => EnemyId == 1304011;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.Book.SetBp(200);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            accumulateDamage = 0;
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            if (accumulateDamage >= owner.MaxHp * 0.15)
                return 0;
            if (accumulateDamage + dmg > owner.MaxHp * 0.15)
            {
                double output = owner.MaxHp*0.15 - accumulateDamage;
                accumulateDamage = owner.MaxHp * 0.15;
                return output;
            }
            accumulateDamage += dmg;
            return base.ChangeDamage(attacker, dmg);
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            if (card?.XmlData?.Spec?.Ranged != CardRange.Near)
                return base.ChangeAttackTarget(card, idx);
            else
                return RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
        }
    }
}

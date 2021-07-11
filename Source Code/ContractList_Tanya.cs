using System;
using System.Collections.Generic;
using System.Linq;
using Sound;
using HarmonyLib;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Tanya_Solo : ContingecyContract
    {
        public ContingecyContract_Tanya_Solo(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { GetParam()};
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Tanya_Solo_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Tanya_Solo_param2");
            if (Level >= 3)
                s += TextDataModel.GetText("Tanya_Solo_param3");
            return s;
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            foreach (BattleDiceCardModel card in this.owner.allyCardDetail.GetAllDeck())
            {
                card.CopySelf();
                DiceCardXmlInfo info = card.XmlData.Copy();
                info.DiceBehaviourList.RemoveAll(x => x.Type == BehaviourType.Standby);
                typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(card, info);
            }
        }
        public BattlePlayingCardDataInUnitModel Retaliate(BattlePlayingCardDataInUnitModel attackerCard)
        {
            if (owner.IsBreakLifeZero())
                return null;
            int cardId = RandomUtil.SelectOne<int>(new int[] { 703601, 703602, 703604, 703611 });
            BattleDiceCardModel card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(cardId));
            BattlePlayingCardDataInUnitModel retaliate = new BattlePlayingCardDataInUnitModel()
            {
                owner = this.owner,
                card = card,
                cardAbility = card.CreateDiceCardSelfAbilityScript(),
                target = attackerCard.owner,
                slotOrder = attackerCard.targetSlotOrder,
                targetSlotOrder = attackerCard.slotOrder
            };
            if (retaliate.cardAbility != null)
                retaliate.cardAbility.card = retaliate;
            retaliate.ResetCardQueue();
            return retaliate;
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            base.OnStartParrying(card);
            if (Level < 1 || Singleton<StageController>.Instance.RoundTurn<=1)
                return;
            card.ApplyDiceStatBonus(DiceMatch.AllAttackDice,new DiceStatBonus() { power = 2 });
            if (Level < 2 || owner.emotionDetail.EmotionLevel<3)
                return;
            card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { power = 2 });
            if (Level < 3 || owner.emotionDetail.EmotionLevel < 5)
                return;
            card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { power = 2 });
        }
    }
    public class ContingecyContract_Tanya_Brawl : ContingecyContract
    {
        private PassiveAbility_1306011 TanyaPassive;
        private Queue<int> Priority;
        private int Ultimate_interval => Level >= 3 ? 3 : 4; 
        private int TanyaPhase => (int)typeof(PassiveAbility_1306011).GetField("_phase", AccessTools.all).GetValue(TanyaPassive);
        public override string[] GetFormatParam => new string[] { GetParam(),(10 + 10 * Level).ToString() };
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s = TextDataModel.GetText("Philip_Silence_param1");
            if (Level >= 2)
                s = TextDataModel.GetText("Philip_Silence_param2");
            if (Level >= 3)
                s = TextDataModel.GetText("Philip_Silence_param3");
            return s;
        }
        
        public ContingecyContract_Tanya_Brawl(int level)
        {
            Level = level - 1;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            TanyaPassive = this.owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1306011) as PassiveAbility_1306011;
            self.bufListDetail.AddBuf(new TanyaMaster(Level));
        }
        public override int SpeedDiceNumAdder() => 2;
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            this.owner.allyCardDetail.ExhaustAllCards();
            this.owner.allyCardDetail.AddNewCard(703603).SetPriorityAdder(Priority.Dequeue());
            int round = Singleton<StageController>.Instance.RoundTurn;
            if (round <= 2)
            {
                SetEarly(round);
                GiveEhanceCard(703604, 703601,3);
                return;
            }
            if (TanyaPhase == 1)
                SetPhase1(round);
            else
                SetPhase2(round);
            GiveEhanceCard(703604, 703601,3);
        }
        private void SetEarly(int round)
        {
            if (round == 1)
            {
                this.owner.allyCardDetail.AddNewCard(703602).SetPriorityAdder(Priority.Dequeue());
                GiveEhanceCard(703602, 703601,2);
                this.owner.allyCardDetail.AddNewCard(703601).SetPriorityAdder(Priority.Dequeue());
            }
            if (round == 2)
            {
                GiveEhanceCard(703602, 703611, 2);
                this.owner.allyCardDetail.AddNewCard(703602).SetPriorityAdder(Priority.Dequeue());
                this.owner.allyCardDetail.AddNewCard(703604).SetPriorityAdder(Priority.Dequeue());
            }
        }
        private void SetPhase1(int round)
        {
            if(round > Ultimate_interval && round % Ultimate_interval == 0)
                this.owner.allyCardDetail.AddNewCard(703612).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703602).SetPriorityAdder(Priority.Dequeue());
            if (round % 2 == 0)
                GiveEhanceCard(703602, 703601, 3);
            else
                this.owner.allyCardDetail.AddNewCard(703604).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703601).SetPriorityAdder(Priority.Dequeue());
            GiveEhanceCard(703611, 703601, 2);
            if (this.owner.emotionDetail.EmotionLevel >= 4)
                GiveEhanceCard(703602, 703611, 3);

        }
        private void SetPhase2(int round)
        {
            if (round > Ultimate_interval && round % Ultimate_interval == 0)
                this.owner.allyCardDetail.AddNewCard(703612).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703602).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703604).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703611).SetPriorityAdder(Priority.Dequeue());
            GiveEhanceCard(703611, 703601, 2);
            if (this.owner.emotionDetail.EmotionLevel >= 4)
                GiveEhanceCard(703602, 703611, 3);
            this.owner.allyCardDetail.AddNewCard(703601).SetPriorityAdder(Priority.Dequeue());
        }
        private void GiveEhanceCard(int cardId,int LowCardId,int Level)
        {
            if(this.Level>= Level)
                this.owner.allyCardDetail.AddNewCard(cardId).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(LowCardId).SetPriorityAdder(Priority.Dequeue());
        }
        public class TanyaMaster: BattleUnitBuf
        {
            private readonly int Level;
            private List<BattleUnitModel> activate;
            private Dictionary<BattleUnitModel,int> info;
            public TanyaMaster(int level)
            {
                Level = level;
            }
            protected override string keywordId => "TanyaMaster";
            protected override string keywordIconId => "Wolf_Claw";
            public override int GetDamageReductionRate() => 10 + 10 * Level;
            public override int GetBreakDamageReductionRate() => 10 + 10 * Level;
            public override string bufActivatedText => Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextDesc(this.keywordId, (object)this.GetAdditionalString());
            private string GetAdditionalString()
            {
                if (activate.Count == 0)
                    return (TextDataModel.GetText("Tanya_Brawl_none"));
                else
                {
                    string s = "";
                    foreach (BattleUnitModel unit in activate)
                        s += unit.UnitData.unitData.name + " ";
                    return s;
                }
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                info = new Dictionary<BattleUnitModel, int>();
                activate = new List<BattleUnitModel>();
                this.stack = 0;
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                    info.Add(unit, 0);
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (activate.Contains(card.target))
                {
                    SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_Bite");
                    card.target.currentDiceAction.ApplyDiceAbility(DiceMatch.AllDice, new ContractReward.DiceCardAbility_invalid());
                }                
                if (info.ContainsKey(card.target))
                    info[card.target] += 1;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                activate.Clear();
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                {
                    if (info.ContainsKey(unit))
                    {
                        if (info[unit] >= 4)
                            activate.Add(unit);
                        info[unit] = 0;
                    }
                    else
                    {
                        info.Add(unit, 0);
                    }
                }
            }
        }
    }
    public class ContingecyContract_Tanya : ContingecyContract
    {
        private PassiveAbility_1306011 TanyaPassive;
        private int activate = 0;
        private bool _nextPhase;
        private int TanyaPhase => (int)typeof(PassiveAbility_1306011).GetField("_phase", AccessTools.all).GetValue(TanyaPassive);
        public ContingecyContract_Tanya(int level)
        {
            Level = level;
        }
        public override bool OnBreakGageZero() => TanyaPhase == 2 && activate == 1;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            TanyaPassive = this.owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1306011) as PassiveAbility_1306011;
        }
        public override bool isImmortal => TanyaPhase == 2 && activate == 0;
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (TanyaPhase == 2 && activate == 0 && (double)this.owner.hp - (double)dmg <= 1.0)
                this._nextPhase = true;
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override void OnRoundEndTheLast()
        {
            if (TanyaPhase == 2 && activate == 1)
                this.owner.RecoverHP((int)(this.owner.MaxHp * 0.10));
            if (TanyaPhase != 2 || activate != 0 || !this._nextPhase && (double)this.owner.hp > 1.0)
                return;
            this.activate = 1;
            this._nextPhase = true;
            this.owner.RecoverHP(this.owner.MaxHp);
            this.owner.RecoverBreakLife(1);
            this.owner.ResetBreakGauge();
            this.owner.breakDetail.nextTurnBreak = false;
            this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
        }
    }
}

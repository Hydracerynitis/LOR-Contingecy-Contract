using HarmonyLib;
using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Contingecy_Contract
{
    public class ContingecyContract_Oswald_Debut : ContingecyContract
    {
        private Queue<int> Priority;
        public ContingecyContract_Oswald_Debut(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam => new string[] { TextDataModel.GetText("Oswald_Debut_param" + Level.ToString()) };
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId==1305021 || EnemyId==1305031;
        private bool IsMermaid => owner.UnitData.unitData.EnemyUnitId == 1305031;
        public override int SpeedDiceNumAdder()
        {
            if (IsMermaid && Level >= 2)
                return 2;
            else if (!IsMermaid && Level >= 3)
                return 1;
            return base.SpeedDiceNumAdder();
        }
        public override void OnDrawCard()
        {
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            if (IsMermaid && Level >= 2)
            {
                owner.allyCardDetail.ExhaustAllCards();
                if (Level >= 3 || Singleton<StageController>.Instance.RoundTurn % 2 == 0)
                    owner.allyCardDetail.AddNewCard(703525).SetPriorityAdder(Priority.Dequeue());
                else
                    owner.allyCardDetail.AddNewCard(703524).SetPriorityAdder(Priority.Dequeue());
                owner.allyCardDetail.AddNewCard(703523).SetPriorityAdder(Priority.Dequeue());
                owner.allyCardDetail.AddNewCard(703522).SetPriorityAdder(Priority.Dequeue());
                owner.allyCardDetail.AddNewCard(703521).SetPriorityAdder(Priority.Dequeue());
            }
            else if(!IsMermaid && Level >= 3)
            {
                owner.allyCardDetail.ExhaustAllCards();
                if (Singleton<StageController>.Instance.RoundTurn % 2 == 0)
                    owner.allyCardDetail.AddNewCard(703514).SetPriorityAdder(Priority.Dequeue());
                else
                    owner.allyCardDetail.AddNewCard(703513).SetPriorityAdder(Priority.Dequeue());
                owner.allyCardDetail.AddNewCard(703512).SetPriorityAdder(Priority.Dequeue());
                owner.allyCardDetail.AddNewCard(703511).SetPriorityAdder(Priority.Dequeue());
            }
            if (IsMermaid)
                return;
            foreach (BattleDiceCardModel card in owner.allyCardDetail.GetHand())
            {
                if (card.GetID() == 703511)
                    DeepCopyUtil.EnhanceCard(card, 3, 4);
                if (card.GetID() == 703512)
                    DeepCopyUtil.EnhanceCard(card, 2, 2,true);
                if (card.GetID() == 703513)
                    DeepCopyUtil.EnhanceCard(card, 0, 8, true);
                if (card.GetID() == 703514)
                {
                    DiceCardXmlInfo xml = card.XmlData.Copy(true);
                    foreach (DiceBehaviour Dice in xml.DiceBehaviourList)
                    {
                        if (Dice.Type == BehaviourType.Standby)
                            continue;
                        Dice.Dice += 2;
                        Dice.Min = Dice.Dice;
                    }
                    typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(card, xml);
                }
            }
        }
    }
    public class ContingecyContract_Oswald_Troll : ContingecyContract
    {
        private Dictionary<BattleUnitModel, int> DPSScore = new Dictionary<BattleUnitModel, int>();
        public ContingecyContract_Oswald_Troll(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam => new string[] { GetParam() };
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1305011;
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Oswald_Troll_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Oswald_Troll_param2");
            if (Level >= 3)
                s += TextDataModel.GetText("Oswald_Troll_param3");
            return s;
        }
        public override void OnRoundEnd()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                if (unit.bufListDetail.GetActivatedBuf(KeywordBuf.OswaldDaze) != null)
                    continue;
                if (DPSScore.ContainsKey(unit))
                    DPSScore[unit] += unit.history.damageAtOneRound;
                else
                    DPSScore.Add(unit, unit.history.damageAtOneRound);
            }
            if (owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1305012) is PassiveAbility_1305012 passive && passive.cardPhase == PassiveAbility_1305012.CardPhase.DO_DAZE)
            {
                List<BattleUnitModel> candicate = new List<BattleUnitModel>();
                int MaxDmg = -1;
                foreach (KeyValuePair<BattleUnitModel, int> pair in DPSScore)
                {
                    if (pair.Key.IsDead())
                        continue;
                    if (pair.Value > MaxDmg)
                    {
                        candicate.Clear();
                        candicate.Add(pair.Key);
                        MaxDmg = pair.Value;
                    }
                    else if (pair.Value == MaxDmg)
                        candicate.Add(pair.Key);
                }
                if (candicate.Count >= 0)
                    RandomUtil.SelectOne<BattleUnitModel>(candicate).bufListDetail.AddBuf(new Trolling(Level));
            }          
        }
        public class TrollIndicator: BattleUnitBuf
        {
            protected override string keywordId => "TrollIndicator";
            protected override string keywordIconId => "Oswald_Daze";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
        public class Trolling: BattleUnitBuf
        {
            private bool _bRecoveredBreak;
            private readonly Vector2Int _formation = new Vector2Int(5, 0);
            private const int _OSWALD_ID = 1305011;
            private SoundEffectPlayer _attractLoopSound;
            private int Level;
            public override bool IsControllable => false;
            protected override string keywordId => "Oswald_Daze";
            public override int SpeedDiceNumAdder()
            {
                return Level>=3? 2:0;
            }
            public override KeywordBuf bufType => KeywordBuf.OswaldDaze;
            public Trolling(int level)
            {
                Level = level;
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 2;
                _bRecoveredBreak = false;
                this.PlayLoopSound(true);
                if (Singleton<StageController>.Instance.AllyFormationDirection == Direction.LEFT)
                {
                    this._owner.formation.ChangePos(_formation);
                }
                else
                {
                    Vector2Int formation = this._formation;
                    formation.x *= -1;
                    this._owner.formation.ChangePos(formation);
                }
            }
            public override void OnRoundStart()
            {
                if (!this._bRecoveredBreak)
                {
                    this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                    this._owner.breakDetail.nextTurnBreak = false;
                    this._owner.turnState = BattleUnitTurnState.WAIT_CARD;
                    this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                    this._bRecoveredBreak = true;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Standing);
                }
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3, this._owner);
                this._owner.cardSlotDetail.RecoverPlayPoint(3);
                if (Level >= 3)
                    _owner.allyCardDetail.DrawCards(2);
                if (_attractLoopSound == null)
                    return;
                SingletonBehavior<BattleSoundManager>.Instance.SetBgmVolumeRatio(0.25f);
            }
            public override bool IsCardChoosable(BattleDiceCardModel card) => (card == null || card.GetSpec().Ranged != CardRange.FarArea) && card.GetSpec().Ranged != CardRange.FarAreaEach && base.IsCardChoosable(card);
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                BattleUnitModel battleUnitModel = (BattleUnitModel)null;
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(_owner.faction);
                aliveList.Remove(_owner);
                if (aliveList.Count > 0)
                    battleUnitModel = aliveList[UnityEngine.Random.Range(0, aliveList.Count)];
                else
                {
                    Singleton<StageController>.Instance.GetStageModel().GetAvailableFloorList().ForEach(x => x.Defeat());
                    Singleton<StageController>.Instance.EndBattle();
                }
                return battleUnitModel;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                DiceStatBonus bonus = new DiceStatBonus() { power = 2 };
                if (Level >= 2)
                {
                    bonus.dmgRate = 50;
                    bonus.breakRate = 50;
                }
                behavior.ApplyDiceStatBonus(bonus);
            }
            public override void OnRoundEnd()
            {
                --this.stack;
                if (this._owner.IsBreakLifeZero())
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                    {
                        if (alive.UnitData.unitData.EnemyUnitId == 1305011)
                            alive.TakeBreakDamage(60, DamageType.Buf);
                    }
                    this.Destroy();
                    this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                    this._owner.breakDetail.nextTurnBreak = false;
                    this._owner.turnState = BattleUnitTurnState.WAIT_CARD;
                    this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                }
                else
                {
                    if (this.stack > 0)
                        return;
                    this.Destroy();
                }
            }
            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                base.OnLoseParrying(behavior);
                this._owner.TakeBreakDamage(this._owner.breakDetail.GetDefaultBreakGauge() / 20, DamageType.Buf);
            }

            public override void OnDie()
            {
                base.OnDie();
                this.PlayLoopSound(false);
                this.Destroy();
            }

            public override void OnDieForReadybuf()
            {
                this.PlayLoopSound(false);
                this.Destroy();
            }

            public override void Destroy()
            {
                base.Destroy();
                BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList().Find((Predicate<BattleUnitModel>)(x => x.UnitData.unitData.EnemyUnitId == 1305011));
                if (battleUnitModel != null)
                {
                    PassiveAbilityBase passiveAbilityBase = battleUnitModel.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_1305012));
                    if (passiveAbilityBase != null)
                        (passiveAbilityBase as PassiveAbility_1305012).IncreaseDazeReleasedCount();
                }
                this._owner.formation.ChangePosToDefault();
                this.PlayLoopSound(false);
            }

            public override void OnEndBattlePhase()
            {
                this._owner.formation.ChangePosToDefault();
                this.PlayLoopSound(false);
            }

            private void PlayLoopSound(bool play)
            {
                if (_attractLoopSound != null)
                {
                    _attractLoopSound.ManualDestroy();
                    _attractLoopSound = (SoundEffectPlayer)null;
                }
                if (play)
                {
                    _attractLoopSound = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Oswald_Attract_Loop", true, parent: this._owner.view.transform);
                    SingletonBehavior<BattleSoundManager>.Instance.SetBgmVolumeRatio(0.25f);
                }
                else
                    SingletonBehavior<BattleSoundManager>.Instance.SetBgmVolumeRatio(1f);
            }
        }
    }
    public class ContingecyContract_Oswald : ContingecyContract
    {
        public ContingecyContract_Oswald(int level)
        {
            Level = level;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (self.UnitData.unitData.EnemyUnitId != 40008)
                return;
            self.formation.ChangePos(new Vector2Int(25, 20));
            self.moveDetail.ReturnToFormationByBlink();    
        }
        public override void OnDrawCard()
        {
            foreach (BattleDiceCardModel card in owner.allyCardDetail.GetHand())
            {
                if (card.GetID() == 503001)
                    DeepCopyUtil.EnhanceCard(card, 1, 1);
                if (card.GetID() == 503002)
                    DeepCopyUtil.EnhanceCard(card, 1, 2);
                if (card.GetID() == 503003)
                    DeepCopyUtil.EnhanceCard(card, 4, 1);
                if (card.GetID() == 503004)
                    DeepCopyUtil.EnhanceCard(card, 2, 2);
                if (card.GetID() == 503008)
                    DeepCopyUtil.EnhanceCard(card, 3, 3);
            }
        }
    }
    public class StageModifier_Oswald : StageModifier
    {
        public StageModifier_Oswald(int Level)
        {
            this.Level = Level;
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.id== 70005;
        }
        public override void Modify(ref StageClassInfo info)
        {
            foreach (StageWaveInfo wave in info.waveList)
            {
                wave.enemyUnitIdList.Add(new LorId(40008));
            }
        }
    }
}

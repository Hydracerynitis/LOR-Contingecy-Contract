using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using BaseMod;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Sound;
using UI;

namespace Contingecy_Contract
{
    public class ContingecyContract_Roland4th_BlackSilence : ContingecyContract
    {
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
        public class PassiveAbility_170301_New : PassiveAbility_170301
        {
            public override void Init(BattleUnitModel self)
            {
                base.Init(self);
                BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60108, 1);
                BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60118, 2);
                BattleUnitModel battleUnitModel3 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60128, 3);
                if (!self.IsBreakLifeZero())
                {
                    self.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                    self.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                    self.view.charAppearance.ChangeMotion(ActionDetail.Default);
                }
                battleUnitModel1.SetDeadSceneBlock(false);
                battleUnitModel1.view.EnableView(false);
                battleUnitModel2.SetDeadSceneBlock(false);
                battleUnitModel2.view.EnableView(false);
                battleUnitModel3.SetDeadSceneBlock(false);
                battleUnitModel3.view.EnableView(false);
            }

            public override void OnRoundEndTheLast()
            {
                if ((double)this.owner.hp <= (double)this.owner.MaxHp * 0.4)
                {
                    SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.SetRunningState(true);
                    this.owner.view.StartCoroutine(this.Transformation());
                }
                else
                {
                    switch (this._patternCount)
                    {
                        case 0:
                            BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60108, 1);
                            BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60118, 2);
                            BattleUnitModel battleUnitModel3 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60128, 3);
                            if (!this.owner.IsBreakLifeZero())
                            {
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                            }
                            battleUnitModel1.SetDeadSceneBlock(false);
                            battleUnitModel1.view.EnableView(false);
                            battleUnitModel2.SetDeadSceneBlock(false);
                            battleUnitModel2.view.EnableView(false);
                            battleUnitModel3.SetDeadSceneBlock(false);
                            battleUnitModel3.view.EnableView(false);
                            break;
                        case 1:
                            BattleUnitModel battleUnitModel4 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60138, 1);
                            BattleUnitModel battleUnitModel5 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60148, 2);
                            BattleUnitModel battleUnitModel6 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60158, 3);

                            if (!this.owner.IsBreakLifeZero())
                            {
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                            }
                            battleUnitModel4.SetDeadSceneBlock(false);
                            battleUnitModel4.view.EnableView(false);
                            battleUnitModel5.SetDeadSceneBlock(false);
                            battleUnitModel5.view.EnableView(false);
                            battleUnitModel6.SetDeadSceneBlock(false);
                            battleUnitModel6.view.EnableView(false);
                            break;
                        case 2:
                            BattleUnitModel battleUnitModel7 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60168, 1);
                            BattleUnitModel battleUnitModel8 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60178, 2);
                            BattleUnitModel battleUnitModel9 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60188, 3);
                            if (!this.owner.IsBreakLifeZero())
                            {
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                                this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                            }
                            battleUnitModel7.SetDeadSceneBlock(false);
                            battleUnitModel7.view.EnableView(false);
                            battleUnitModel8.SetDeadSceneBlock(false);
                            battleUnitModel8.view.EnableView(false);
                            battleUnitModel9.SetDeadSceneBlock(false);
                            battleUnitModel9.view.EnableView(false);
                            break;
                        case 3:
                            if (!this.owner.IsBreakLifeZero())
                            {
                                this.owner.view.charAppearance.ChangeMotion(ActionDetail.S12);
                                break;
                            }
                            break;
                    }
                    int num = 0;
                    foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList())
                        SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(unit.UnitData.unitData, num++, true);
                    BattleObjectManager.instance.InitUI();
                }
            }

            public override void OnRoundStartAfter()
            {
                if (this.owner.IsBreakLifeZero())
                    return;
                this.SetNewCards();
                switch (this._patternCount)
                {
                    case 0:
                        this.owner.passiveDetail.AddPassive(new PassiveAbility_170304());
                        this.owner.passiveDetail.OnCreated();
                        break;
                    case 1:
                        ++this._strCnt;
                        this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._strCnt, this.owner);
                        this.owner.passiveDetail.AddPassive(new PassiveAbility_170305());
                        this.owner.passiveDetail.OnCreated();
                        break;
                    case 2:
                        this.owner.passiveDetail.AddPassive(new PassiveAbility_170306());
                        this.owner.passiveDetail.OnCreated();
                        break;
                    case 3:
                        this.owner.passiveDetail.AddPassive(new PassiveAbility_170307());
                        this.owner.passiveDetail.OnCreated();
                        break;
                }
                ++this._patternCount;
                this._patternCount %= 4;
                switch (this._currentBuf)
                {
                    case BehaviourDetail.Slash:
                        this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Slash());
                        this._currentBuf = BehaviourDetail.Penetrate;
                        break;
                    case BehaviourDetail.Penetrate:
                        this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Penetrate());
                        this._currentBuf = BehaviourDetail.Hit;
                        break;
                    case BehaviourDetail.Hit:
                        this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Hit());
                        this._currentBuf = BehaviourDetail.Slash;
                        break;
                }
            }

            private void SetNewCards()
            {
                this.owner.allyCardDetail.ExhaustAllCards();
                int num = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count - 3;
                this._cardCount = 0;
                switch (this._patternCount)
                {
                    case 0:
                        this.AddNewCard(702301);
                        this.AddNewCard(702302);
                        this.AddNewCard(702303);
                        this.AddNewCard(702315);
                        this.AddNewCard(702316);
                        break;
                    case 1:
                        this.AddNewCard(702304);
                        this.AddNewCard(702305);
                        this.AddNewCard(702306);
                        this.AddNewCard(702315);
                        this.AddNewCard(702316);
                        break;
                    case 2:
                        this.owner.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S14);
                        this.owner.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S14);
                        this.owner.view.charAppearance.ChangeMotion(ActionDetail.S14);
                        this.AddNewCard(702307);
                        this.AddNewCard(702308);
                        this.AddNewCard(702309);
                        this.AddNewCard(702315);
                        this.AddNewCard(702316);
                        break;
                    case 3:
                        this.owner.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S14);
                        this.owner.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S14);
                        this.owner.view.charAppearance.ChangeMotion(ActionDetail.S14);
                        this.Map?.AttatchAura();
                        this.AddNewCard(702313);
                        this.AddNewCard(702310);
                        this.AddNewCard(702312);
                        this.AddNewCard(702312);
                        this.AddNewCard(702315);
                        break;
                }
                for (int index = 0; index < num; ++index)
                    this.AddNewCard(this.GetAddedDiceCard());
            }
        }
    }
    public class ContingecyContract_Roland4th_Servant : ContingecyContract
    {
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
                behavior.owner.battleCardResultLog?.SetPassiveAbility(this);
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
    }
    public class StageModifier_Roland : StageModifier
    {
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

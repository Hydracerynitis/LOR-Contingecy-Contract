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
